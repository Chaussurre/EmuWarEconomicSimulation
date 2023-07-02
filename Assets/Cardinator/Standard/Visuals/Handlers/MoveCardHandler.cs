using System;
using System.Collections;
using UnityEngine;

namespace Cardinator.Standard
{
    public class MoveCardHandler : InteractionHandler
    {
        public SortingLayerPicker MovingLayer;
        public SortingLayerPicker NonMovingLayer;
        
        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Grab)
            {
                data.Handler.StackVisual.SetControl(data.Target, false);
                (data.Target as CardVisual).canvas.sortingLayerID = MovingLayer.id;
            }

            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Hold)
            {
                data.Target.MoveTo(data.MousePosition);
            }

            if (data.LeftClickStatus == CardStackVisualHandler<CardData>.ClickStatus.Drop)
            {
                data.Handler.StackVisual.SetControl(data.Target, true);
                (data.Target as CardVisual).canvas.sortingLayerID = NonMovingLayer.id;
            }
        }
    }
}