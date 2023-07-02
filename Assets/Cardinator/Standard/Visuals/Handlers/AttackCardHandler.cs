using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cardinator.Standard.Assets.Tabletop.Standard.Visuals.Handlers
{
    public class AttackCardHandler : InteractionHandler
    {
        public CardStackVisual TargetStackVisual;
        public PlayerManager PlayerManager;

        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.LeftClickStatus != CardStackVisualHandler<CardData>.ClickStatus.Drop)
                return;

            var target = TargetStackVisual.FirstOrDefault(x => x.IsPointOnCard(data.MousePosition) &&
             TargetStackVisual.CardStack.Contains(x.CardID));

            if (!target)
                return;

            var player = PlayerManager.ActivePlayer;
            player.Attack(data.Target.CardID, target.CardID);
        }
    }
}