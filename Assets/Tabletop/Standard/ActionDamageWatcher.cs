using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public struct DamageData
    {
        public int CardID;
        public int damages;
    }

    public class ActionDamageWatcher : ActionWatcher<StandardCardData, DamageData>
    {
        protected override void Apply(DamageData actionData)
        {

            if (!manager.CardPool.PositionTracker.TryGetValue(actionData.CardID, out var cardPos))
                return;

            var card = cardPos.GetCard();

            card.data.Hp -= Mathf.Min(actionData.damages, card.data.Hp);

            cardPos.stack.UpdateCard(card, cardPos.index);
        }
    }
}
