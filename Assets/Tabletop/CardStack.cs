using CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Tabletop
{
    public abstract class CardStack<TCardData> : NetworkBehaviour where TCardData : struct
    {
        public struct CardPosition
        {
            public CardStack<TCardData> stack;
            public int index;

            public Card<TCardData>.CardInstance GetCard()
            {
                return stack.GetCard(index);
            }
        }

        public struct CardStackDataChange
        {
            public enum ChangeType
            {
                CREATE, DESTROY, UPDATE, CLEAR
            }

            public int CardChangedIndex;
            public ChangeType Change;
            public Card<TCardData>.CardInstance OldCard;
            public Card<TCardData>.CardInstance NewCard;
        }

        SyncList<Card<TCardData>.CardInstance> Cards = new();
        public DataWatcher<CardStackDataChange> DataWatcher;
        public CardPool<TCardData> CardPool;

        private void Awake()
        {
            if (CardPool == null)
                CardPool = FindObjectOfType<CardPool<TCardData>>();
        }

        public int Size => Cards.Count;

        public Card<TCardData>.CardInstance GetCard(int index) => Cards[index];

        [Server]
        public virtual void Clear()
        {
            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.CLEAR,
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            foreach (var card in Cards)
                CardPool.PositionTracker.Remove(card.CardID);

            Cards.Clear();

            DataChangeReact(CardChangeData);
        }

        [Server]
        public virtual void AddCard(Card<TCardData>.CardInstance card)
        {
            InsertCard(card, Cards.Count);
        }

        [Server]
        public virtual void InsertCard(Card<TCardData>.CardInstance card, int index)
        {
            if (index < 0 || index > Cards.Count)
                throw new ArgumentOutOfRangeException("index");

            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.CREATE,
                CardChangedIndex = index,
                NewCard = card,
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            if (CardChangeData.CardChangedIndex < 0 || CardChangeData.CardChangedIndex > Cards.Count)
                throw new ArgumentOutOfRangeException("CardChangeData.CardChangedIndex");

            Cards.Insert(CardChangeData.CardChangedIndex, CardChangeData.NewCard);
            UpdateTracker();

            DataChangeReact(CardChangeData);
        }

        [Server]
        public virtual void UpdateCard(Card<TCardData>.CardInstance card, int index)
        {
            if (index < 0 || index >= Cards.Count)
                throw new ArgumentOutOfRangeException("index");

            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.UPDATE,
                CardChangedIndex = index,
                OldCard = Cards[index],
                NewCard = card,
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            if (CardChangeData.CardChangedIndex < 0 || CardChangeData.CardChangedIndex >= Cards.Count)
                throw new ArgumentOutOfRangeException("CardChangeData.CardChangedIndex");

            Cards[CardChangeData.CardChangedIndex] = CardChangeData.NewCard;

            CardPool.PositionTracker.Remove(CardChangeData.OldCard.CardID);
            UpdateTracker();

            DataChangeReact(CardChangeData);
        }

        [Server]
        public virtual void RemoveCard(int index)
        {
            if (index < 0 || index >= Cards.Count)
                throw new ArgumentOutOfRangeException("index");

            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.DESTROY,
                CardChangedIndex = index,
                OldCard = Cards[index],
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            if (CardChangeData.CardChangedIndex < 0 || CardChangeData.CardChangedIndex >= Cards.Count)
                throw new ArgumentOutOfRangeException("CardChangeData.CardChangedIndex");

            Cards.RemoveAt(CardChangeData.CardChangedIndex);
            CardPool.PositionTracker.Remove(CardChangeData.OldCard.CardID);
            UpdateTracker();

            DataChangeReact(CardChangeData);
        }

        private void UpdateTracker()
        {
            for(int i = 0; i < Cards.Count; i++)
            {
                var card = GetCard(i);
                var pos = new CardPosition()
                {
                    stack = this,
                    index = i,
                };
                CardPool.PositionTracker[card.CardID] = pos;
            }
        }

        protected abstract void DataChangeReact(CardStackDataChange dataChange);
    }
}
