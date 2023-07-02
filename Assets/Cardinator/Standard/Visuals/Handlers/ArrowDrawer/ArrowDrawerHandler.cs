using System.Collections;
using UnityEngine;

namespace Cardinator.Standard.Assets.Tabletop.Standard.Visuals.Handlers.ArrowDrawer
{
    public class ArrowDrawerHandler : MonoBehaviour
    {
        public Arrow Arrow;

        public void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Drop)
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