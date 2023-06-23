using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class CardVisualManager<TCardData> where TCardData : struct
    {
        public CardPool<TCardData> CardPool;

        private Dictionary<int, CardVisual<TCardData>> VisualTracker = new();

        public CardVisualManager(CardPool<TCardData> cardPool)
        {
            CardPool = cardPool;
        }

        public CardVisual<TCardData> GetVisual(Card<TCardData>.CardInstance card)
        {
            if (VisualTracker.TryGetValue(card.CardID, out var visual))
                return visual;

            var model = CardPool.GetCard(card);

            return model.CreateVisual(card, CardPool);
        }

        public void DeleteVisual(int CardID)
        {
            if (VisualTracker.TryGetValue(CardID, out var value))
                GameObject.Destroy(value.gameObject);

            VisualTracker.Remove(CardID);
        }
    }
}