using System;
using System.Collections;
using UnityEngine;

namespace Tabletop.Standard
{
    public class MoveCardHandler : InteractionHandler
    {

        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Grab)
            {
                data.Handler.StackVisual.SetControl(data.Target, false);
            }

            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Hold)
            {
                data.Target.MoveTo(data.MousePosition);
            }

            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Drop)
            {
                data.Handler.StackVisual.SetControl(data.Target, true);
            }
        }
    }
}