using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    [Serializable]
    public class CardVisualManager<TCardData> where TCardData : struct
    {
        [Tooltip("If this is on, when all actions and their visuals are done, this will remake all visuals at once." +
            " Useful if not all actions have visuals")]
        public bool DumpVisualsOnActionsEnd;

        private CardManager<TCardData> CardManager;

        private Dictionary<int, CardVisual<TCardData>> VisualTracker = new();
        private Dictionary<CardStack<TCardData>, CardStackVisual<TCardData>> StackVisuals = new();

        private List<int> DumpBuffer = new();

        public void DumpVisuals()
        {
            if (!DumpVisualsOnActionsEnd)
                return;

            foreach(var cardID in VisualTracker.Keys)
                DumpBuffer.Add(cardID);

            foreach (var stack in StackVisuals.Keys)
            {
                var stackVisual = StackVisuals[stack];

                for (int i = 0; i < stack.Size; i++)
                {
                    var card = stack.GetCard(i);

                    if (i >= stackVisual.Count)
                        stackVisual.InsertCard(card.Value, i);
                    else
                        stackVisual.SetCard(card.Value, i);
                    DumpBuffer.Remove(card.Value.CardID);
                }

                for (int i = stackVisual.Count - 1; i >= stack.Size; i--)
                    stackVisual.RemoveCard(i);
            }

            foreach (var cardID in DumpBuffer)
                DeleteVisual(cardID);

            DumpBuffer.Clear();
        }

        internal void Init(CardManager<TCardData> cardManager)
        {
            CardManager = cardManager;
            cardManager.ActionsManager.OnResolved.AddListener(DumpVisuals);
        }

        public CardVisual<TCardData> GetVisual(Card<TCardData>.CardInstance card)
        {
            return GetVisual(card.CardID);
        }
        public CardVisual<TCardData> GetVisual(int cardID)
        {
            if (VisualTracker.TryGetValue(cardID, out var visual))
                return visual;

            var Card = CardManager.GetCardInstance(cardID);

            if (!Card.HasValue)
                return null;

            var model = CardManager.CardPool.GetCard(Card.Value);
            var createdVisual = model.CreateVisual(Card.Value);

            VisualTracker.Add(cardID, createdVisual);

            return createdVisual;
        }

        public void DeleteVisual(int CardID)
        {
            if (VisualTracker.TryGetValue(CardID, out var value))
                GameObject.Destroy(value.gameObject);

            VisualTracker.Remove(CardID);
        }

        public void RegisterStack(CardStack<TCardData> stack, CardStackVisual<TCardData> stackVisual)
        {
            StackVisuals.Add(stack, stackVisual);
        }
    }
}