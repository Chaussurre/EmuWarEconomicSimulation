using System;
using UnityEngine;

namespace Tabletop
{
    public class Card<TCardData> : MonoBehaviour where TCardData : struct
    {
        [Serializable]
        public struct CardInstance
        {
            public int CardModelID;
            public int CardID;
            public PlayerMask VisibleMask;
            public TCardData data;
        }

        [SerializeField] private CardVisual<TCardData> VisualPrefab;

        public TCardData DefaultData;

        public CardVisual<TCardData> CreateVisual(CardInstance Card)
        {
            var visual = Instantiate(VisualPrefab);
            visual.UpdateData(Card.data);
            visual.CardID = Card.CardID;

            return visual;
        }
    }
}