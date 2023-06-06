using System;
using UnityEngine;

namespace Tabletop
{
    public class Card<TCardData> : MonoBehaviour where TCardData : struct
    {
        [Serializable]
        public struct CardInstance
        {
            public int ID;
            public TCardData data;
        }

        [SerializeField] private CardVisual<TCardData> VisualPrefab;

        public CardVisual<TCardData> CreateVisual(CardInstance Card, CardManager<TCardData> manager)
        {
            var visual = Instantiate(VisualPrefab);
            visual.InitID(Card.ID, manager);
            visual.UpdateData(Card);
            return visual;
        }
    }
}