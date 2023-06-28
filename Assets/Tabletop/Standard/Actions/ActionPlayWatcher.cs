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
            var cardInstance = CardManager.GetCardInstance(actionData.CardID);

            if (!cardInstance.HasValue)
                return;

            var cardModel = CardManager.CardPool.GetCard(cardInstance.Value) as Card;

            cardModel.OnPlay?.Invoke(CardManager, cardInstance.Value);

            if (cardModel.IsAUnit)
            {
                var summonData = ActionSummonWatcher.Summon(cardInstance.Value.CardID, cardInstance.Value.data);
                CardManager.ActionsManager.AddActionImmediate(summonData);
            }
        }

        public static PlayData Play(int CardID)
        {
            return new() { CardID = CardID };
        }
    }
}
