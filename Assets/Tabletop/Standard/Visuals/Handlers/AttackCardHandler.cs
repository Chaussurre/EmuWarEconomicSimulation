using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop.Standard.Assets.Tabletop.Standard.Visuals.Handlers
{
    public class AttackCardHandler : InteractionHandler
    {
        public CardStackVisual TargetStackVisual;

        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus != CardStackVisualHandler<CardData>.ClickStatus.Drop)
                return;

            var target = TargetStackVisual.First(x => x.IsPointOnCard(data.MousePosition) &&
             TargetStackVisual.CardStack.Contains(x.CardID));

            var strikeData = ActionStrikeWatcher.Fight(data.Target.CardID, target.CardID);
            data.CardManager.ActionsManager.AddAction(strikeData);
        }
    }
}