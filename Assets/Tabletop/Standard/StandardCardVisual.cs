using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tabletop.Standard
{
    public class StandardCardVisual : CardVisual<StandardCardData>
    {
        public Canvas canvas;

        public TMP_Text TextHp;
        public TMP_Text TextAttack;
        public TMP_Text TextCost;

        public override void UpdateData(StandardCardData cardData)
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
