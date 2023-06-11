using System;
using System.Collections.Generic;
using UnityEngine;


namespace Tabletop
{
    public class CardPool<TCardData> : MonoBehaviour where TCardData : struct
    {
        public List<Card<TCardData>> Cards = new();
        public Card<TCardData> HiddenCardTemplate;

        public Card<TCardData> GetCard(Card<TCardData>.CardInstance instance)
        {
            if (instance.CardID < 0)
                return HiddenCardTemplate;

            return Cards[instance.CardID];
        }

        public CardVisual<TCardData> CreateVisual(Card<TCardData>.CardInstance card)
        {
            return GetCard(card).CreateVisual(card, this);
        }

        public Card<TCardData> GetHiddenCardTemplate() => HiddenCardTemplate;

        public Card<TCardData>.CardInstance CreateInstance(Card<TCardData> Card)
        {
            if (!Cards.Contains(Card))
                throw new ArgumentOutOfRangeException("Card");

            return CreateInstanceFromIndex(Cards.IndexOf(Card), null);
        }
        public Card<TCardData>.CardInstance CreateInstance(Card<TCardData> Card, TCardData data)
        {
            if (!Cards.Contains(Card))
                throw new ArgumentOutOfRangeException("Card");

            return CreateInstanceFromIndex(Cards.IndexOf(Card), data);
        }
        public Card<TCardData>.CardInstance CreateInstance(string name)
        {
            var index = NameIndex(name);

            if (index == -1)
                throw new ArgumentOutOfRangeException("name");

            return CreateInstanceFromIndex(index, null);
        }

        public Card<TCardData>.CardInstance CreateInstance(string name, TCardData data)
        {
            var index = NameIndex(name);

            if (index == -1)
                throw new ArgumentOutOfRangeException("name");

            return CreateInstanceFromIndex(index, data);
        }


        protected int NameIndex(string name)
        {
            for (int i = 0; i < Cards.Count; i++)
                if (Cards[i].name == name)
                    return i;

            return -1;
        }

        private Card<TCardData>.CardInstance CreateInstanceFromIndex(int index, TCardData? data)
        {
            return new()
            {
                CardID = index,
                hidden = false,
                data = data ?? Cards[index].DefaultData,
            };
        }
    }
}
