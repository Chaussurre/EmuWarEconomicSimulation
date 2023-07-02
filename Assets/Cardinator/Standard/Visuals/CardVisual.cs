using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cardinator.Standard
{
    public class CardVisual : CardVisual<CardData>
    {
        public Canvas canvas;

        public TMP_Text TextHp;
        public TMP_Text TextAttack;
        public TMP_Text TextCost;

        public override void UpdateData(CardData cardData)
        {
            TextHp.text = cardData.Hp.ToString();
            TextAttack.text = cardData.Attack.ToString();
            TextCost.text = cardData.cost.ToString();

        }
        public override void SetRenderingOrder(int order)
        {
            canvas.sortingOrder = order;
        }
    }
}
