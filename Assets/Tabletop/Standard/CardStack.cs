using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class CardStack : CardStack<CardData> 
    {
        [SerializeField] protected CardManager CardManager;
        [SerializeField] List<Card> InitialCards;
        [SerializeField] bool CreateVisible;

        private void Start()
        {
            foreach (var card in InitialCards)
                CardManager.CreateInstance(card, new(this), VisibleTo: CreateVisible? null : PlayerMask.Empty);


            CardManager.VisualManager.DumpVisuals();
        }
    }
}
