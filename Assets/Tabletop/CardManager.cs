using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class CardManager<TCardData> : MonoBehaviour where TCardData : struct
    {
        public CardPool<TCardData> CardPool = new();

        public CardVisualManager<TCardData> VisualManager;

        public ActionsManager<TCardData> ActionsManager;

        private int InstanceIDIncrementer = 0;

        private Dictionary<int, CardStack<TCardData>> PositionTracker = new();

        private void Awake()
        {
            ActionsManager.Init(GetComponentsInChildren<IActionWatcher<TCardData>>(), this);
            VisualManager.Init(this);
        }

        public Dictionary<int, CardStack<TCardData>>.KeyCollection Cards => PositionTracker.Keys;

        public Card<TCardData>.CardInstance? CreateInstance(Card<TCardData> Card, CardStack<TCardData>.CardPosition position, TCardData? data = null)
        {
            if (!CardPool.Cards.Contains(Card))
                throw new ArgumentOutOfRangeException("Card");

            if (!position.CanInsert())
                return null;

            var index = CardPool.GetCardIndex(Card);

            Card<TCardData>.CardInstance instance =  new()
            {
                CardModelID = index,
                CardID = InstanceIDIncrementer++,
                hidden = false,
                data = data ?? Card.DefaultData,
            };

            position.InsertCard(instance);
            PositionTracker[instance.CardID] = position.stack;

            return instance;
        }

        public bool DestroyInstance(int CardID)
        {
            var pos = GetCardPos(CardID);
            if (!pos.HasValue)
                return false; // CardID does not exists

            pos.Value.RemoveCard();
            PositionTracker.Remove(CardID);

            return true;
        }

        public bool MoveCard(int CardID, CardStack<TCardData>.CardPosition destination)
        {
            if (!destination.CanInsert())
                return false; //destination pos is not valid

            var fromNullable = GetCardPos(CardID);
            if (!fromNullable.HasValue)
                return false; // CardID doesn't exist

            var from = fromNullable.Value;
            var card = from.GetCard().Value;
            from.RemoveCard();
            destination.InsertCard(card);

            PositionTracker[CardID] = destination.stack;
            
            return true;
        }

        public bool UpdateCard(int CardID, TCardData NewData)
        {
            var pos = GetCardPos(CardID);
            if (!pos.HasValue)
                return false; //CardID doesn't exist

            return pos.Value.UpdateCard(NewData);
        }

        public bool TransformCard(int CardID, Card<TCardData> NewModel)
        {
            var pos = GetCardPos(CardID);
            if (!pos.HasValue)
                return false; //CardID doesn't exist

            var card = pos.Value.GetCard().Value;

            var modelID = CardPool.GetCardIndex(NewModel);

            if (modelID == -1)
                return false; //Model isn't valid

            return pos.Value.SetCard(new()
            {
                CardModelID = modelID,
                CardID = card.CardID,
                hidden = card.hidden,
                data = card.data,
            });
        }

        public CardStack<TCardData>.CardPosition? GetCardPos(int CardID)
        {
            if (PositionTracker.TryGetValue(CardID, out var stack))
                return stack.GetCardPos(CardID);

            return null;
        }

        public Card<TCardData>.CardInstance? GetCardInstance(int CardID)
        {
            return GetCardPos(CardID)?.GetCard();
        }
    }
}