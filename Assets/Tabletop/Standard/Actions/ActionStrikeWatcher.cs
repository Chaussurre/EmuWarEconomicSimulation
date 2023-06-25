using System;

namespace Tabletop.Standard
{
    public class ActionStrikeWatcher : ActionWatcher<StandardCardData, ActionStrikeWatcher.StrikeData>
    {
        [Serializable]
        public struct StrikeData
        {
            public int CardID1;
            public int CardID2;
            public bool From1to2;
            public bool From2to1;
        }

        protected override void Apply(StrikeData actionData)
        {
            var Card1Pos = manager.GetCardPos(actionData.CardID1);
            var Card2Pos = manager.GetCardPos(actionData.CardID2);

            if (!Card1Pos.HasValue || !Card2Pos.HasValue)
                return;

            var card1 = Card1Pos.Value.GetCard().Value;
            var card2 = Card2Pos.Value.GetCard().Value;

            if (actionData.From1to2 && card1.data.Attack > 0)
                manager.ActionsManager.AddAction(ActionDamageWatcher.DamageData.DealDamage(card2.CardID, card1.data.Attack, card1.CardID));

            if (actionData.From2to1 && card2.data.Attack > 0)
                manager.ActionsManager.AddAction(ActionDamageWatcher.DamageData.DealDamage(card1.CardID, card2.data.Attack, card2.CardID));
        }
    }
}
