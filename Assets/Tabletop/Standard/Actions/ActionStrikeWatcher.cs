using System;

namespace Tabletop.Standard
{
    public class ActionStrikeWatcher : ActionWatcher<CardData, ActionStrikeWatcher.StrikeData>
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
            var Card1Pos = CardManager.GetCardPos(actionData.CardID1);
            var Card2Pos = CardManager.GetCardPos(actionData.CardID2);

            if (!Card1Pos.HasValue || !Card2Pos.HasValue)
                return;

            var card1 = Card1Pos.Value.GetCard().Value;
            var card2 = Card2Pos.Value.GetCard().Value;

            if (actionData.From1to2 && card1.data.Attack > 0)
            {
                var damageData = ActionDamageWatcher.DealDamage(card2.CardID, card1.data.Attack, card1.CardID);
                CardManager.ActionsManager.AddAction(damageData);
            }

            if (actionData.From2to1 && card2.data.Attack > 0)
            {
                var damageData = ActionDamageWatcher.DealDamage(card1.CardID, card2.data.Attack, card2.CardID);
                CardManager.ActionsManager.AddAction(damageData);
            }
        }

        public static StrikeData Strike(int StrikerID, int StrikedID)
        {
            return new()
            {
                CardID1 = StrikerID,
                CardID2 = StrikedID,
                From1to2 = true,
                From2to1 = false,
            };
        }

        public static StrikeData Fight(int Fighter1ID, int Fighter2ID)
        {
            return new()
            {
                CardID1 = Fighter1ID,
                CardID2 = Fighter2ID,
                From1to2 = true,
                From2to1 = true,
            };
        }
    }
}
