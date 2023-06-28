using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop.Standard.Assets.Tabletop.Standard.Visuals.Handlers
{
    public class AttackCardHandler : InteractionHandler
    {
        public CardStackVisual TargetStackVisual;

        public override void OnCardInteract(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus != CardStackVisualHandler<StandardCardData>.ClickStatus.Drop)
                return;

            var target = TargetStackVisual.First(x => x.IsPointOnCard(data.MousePosition) &&
             TargetStackVisual.CardStack.Contains(x.CardID));

            data.CardManager.ActionsManager.AddAction(new ActionStrikeWatcher.StrikeData()
            {
                CardID1 = data.Target.CardID,
                CardID2 = target.CardID,
                From1to2 = true,
                From2to1 = true,
            });
        }
    }
}