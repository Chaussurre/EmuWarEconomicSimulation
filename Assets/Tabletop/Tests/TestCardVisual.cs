using TMPro;

namespace Tabletop
{
    public class TestCardVisual : CardVisual<int>
    {
        public TMP_Text text;

        protected override void UpdateInternData(int cardData)
        {
            text.text = cardData.ToString();
        }
    }
}
