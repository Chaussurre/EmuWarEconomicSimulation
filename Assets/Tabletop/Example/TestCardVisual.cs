using TMPro;
using UnityEngine;

namespace Tabletop.Example
{
    public class TestCardVisual : CardVisual<int>
    {
        public TMP_Text text;

        public SpriteRenderer spriteRenderer;

        protected override void UpdateInternData(int cardData)
        {
            text.text = cardData.ToString();
        }
        public override void SetRenderingOrder(int order)
        {
            text.canvas.sortingOrder = order;
            spriteRenderer.sortingOrder = order;
        }
    }
}
