using System;

namespace Tabletop.Standard
{
    public class ActionPlayWatcher : ActionWatcher<CardData, ActionPlayWatcher.PlayData>
    {
        [Serializable]
        public struct PlayData
        {
            public int CardID;
        }

        public CardStack PlayingField;

        protected override void Apply(PlayData actionData)
        {
            var cardInstance = manager.GetCardInstance(actionData.CardID);

            if (!cardInstance.HasValue)
                return;

            var cardModel = manager.CardPool.GetCard(cardInstance.Value) as Card;

            cardModel.OnPlay?.Invoke(manager, cardInstance.Value);

            if (cardModel.IsAUnit)
            {
                var summonData = ActionSummonWatcher.Summon(cardInstance.Value.CardID, cardInstance.Value.data);
                manager.ActionsManager.AddActionImmediate(summonData);
            }
        }

        public static PlayData Play(int CardID)
        {
            return new() { CardID = CardID };
        }
    }
}
