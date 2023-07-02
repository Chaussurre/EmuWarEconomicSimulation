using System;
using System.Collections.Generic;
using UnityEngine;


namespace Cardinator
{
    [Serializable]
    public class CardPool<TCardData> where TCardData : struct
    {
        public List<Card<TCardData>> Cards = new();
        public Card<TCardData> HiddenCardTemplate;

        public Card<TCardData> GetCard(Card<TCardData>.CardInstance instance)
        {
            return GetCard(instance.CardModelID);
        }

        public Card<TCardData> GetCard(int ModelID)
        {
            if (ModelID < 0)
                return HiddenCardTemplate;

            return Cards[ModelID];
        }

        public Card<TCardData> GetCard(string name)
        {
            foreach (var card in Cards)
                if (card.name == name)
                    return card;
            return HiddenCardTemplate;
        }

        public int GetCardIndex(Card<TCardData> Card)
        {
            if (!Cards.Contains(Card))
                return -1;

            return Cards.IndexOf(Card);
        }

        public Card<TCardData> GetHiddenCardTemplate() => HiddenCardTemplate;
    }
}
