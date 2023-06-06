using TMPro;

namespace Tabletop
{
    public class TestCardVisual : CardVisual<int>
    {
        public TMP_Text text;

        public override void UpdateData(int cardData)
        {
            text.text = cardData.ToString();
        }
    }
}
