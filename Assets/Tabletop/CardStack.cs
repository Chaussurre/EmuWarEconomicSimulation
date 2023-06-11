using CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class CardStack<TCardData> : MonoBehaviour where TCardData : struct
    {
        public struct CardStackDataChange
        {
            public enum ChangeType
            {
                ADD, REMOVE, UPDATE
            }

            public int CardChangedIndex;
            public ChangeType Change;
            public Card<TCardData>.CardInstance OldCard;
            public Card<TCardData>.CardInstance NewCard;
        }

        List<Card<TCardData>.CardInstance> Cards = new();
        public DataWatcher<CardStackDataChange> DataWatcher;
        public CardPool<TCardData> CardPool;

        private void Awake()
        {
            if (CardPool == null)
                CardPool = FindObjectOfType<CardPool<TCardData>>();
        }

        public int Size => Cards.Count;

        public Card<TCardData>.CardInstance GetCard(int index) => Cards[index];


        public void AddCard(Card<TCardData>.CardInstance card)
        {
            InsertCard(card, Cards.Count);
        }

        public void InsertCard(Card<TCardData>.CardInstance card, int index)
        {
            if (index < 0 || index > Cards.Count)
                throw new ArgumentOutOfRangeException("index");

            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.ADD,
                CardChangedIndex = index,
                NewCard = card,
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            if (CardChangeData.CardChangedIndex < 0 || CardChangeData.CardChangedIndex > Cards.Count)
                throw new ArgumentOutOfRangeException("CardChangeData.CardChangedIndex");

            Cards.Insert(CardChangeData.CardChangedIndex, CardChangeData.NewCard);
        }

        public void UpdateCard(Card<TCardData>.CardInstance card, int index)
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
        }

        public void RemoveCard(int index)
        {
            if (index < 0 || index >= Cards.Count)
                throw new ArgumentOutOfRangeException("index");

            CardStackDataChange CardChangeData = new()
            {
                Change = CardStackDataChange.ChangeType.REMOVE,
                CardChangedIndex = index,
                OldCard = Cards[index],
            };

            CardChangeData = DataWatcher.WatchData(CardChangeData);

            if (CardChangeData.CardChangedIndex < 0 || CardChangeData.CardChangedIndex >= Cards.Count)
                throw new ArgumentOutOfRangeException("CardChangeData.CardChangedIndex");

            Cards.RemoveAt(CardChangeData.CardChangedIndex);
        }
    }
}
