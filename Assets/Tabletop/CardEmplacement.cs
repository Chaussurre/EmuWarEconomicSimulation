using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class CardEmplacement<TCardData> : MonoBehaviour where TCardData : struct
    {
        Card<TCardData>.CardInstance? Card;
        CardVisual<TCardData> CardVisual;

        public void AddCard(Card<TCardData>.CardInstance card, CardManager<TCardData> manager)
        {
            Card = card;
            UpdateVisuals(manager);
        }

        public Card<TCardData>.CardInstance? GetCard() => Card;

        public void RemoveCard(CardManager<TCardData> manager)
        {
            Card = null;
            UpdateVisuals(manager);
        }

        public void UpdateVisuals(CardManager<TCardData> manager)
        {
            if (!Card.HasValue && CardVisual)
            {
                Destroy(CardVisual.gameObject);
                CardVisual = null;

                return;
            }

            if (CardVisual)
            {
                CardVisual.UpdateData(Card.Value.data);
                return;
            }

            CardVisual = Instantiate(manager.GetCard(Card.Value).VisualPrefab, transform);
            CardVisual.UpdateData(Card.Value.data);
        }
    }
}
