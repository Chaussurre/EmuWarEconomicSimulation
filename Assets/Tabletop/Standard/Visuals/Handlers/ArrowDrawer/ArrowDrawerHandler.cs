using System.Collections;
using UnityEngine;

namespace Tabletop.Standard.Assets.Tabletop.Standard.Visuals.Handlers.ArrowDrawer
{
    public class ArrowDrawerHandler : MonoBehaviour
    {
        public Arrow Arrow;

        public void OnCardInteract(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus == CardStackVisualHandler<StandardCardData>.ClickStatus.Drop)
            {
                Arrow.gameObject.SetActive(false);
            }

            if (data.isClicking(0))
            {
                Arrow.gameObject.SetActive(true);

                Arrow.destination = data.MousePosition;
                Arrow.from = data.Target.transform.position;
            }
        }
    }
}