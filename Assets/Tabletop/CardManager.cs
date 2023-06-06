using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tabletop
{
    public class CardManager<TCardData> : MonoBehaviour where TCardData : struct
    {
        [SerializeField] private List<Card<TCardData>> Cards = new();

        public Card<TCardData> GetCard(Card<TCardData>.CardInstance instance) => Cards[instance.ID];
    }
}
