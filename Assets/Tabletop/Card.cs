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

        public CardVisual<TCardData> VisualPrefab;
    }
}