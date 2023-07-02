using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class PlayField : MonoBehaviour
    {
        [Serializable]
        public struct PlayerField
        {
            public CardStack Deck;
            public CardStack Hand;
            public CardStack Field;
        }

        public List<PlayerField> Fields;
    }
}
