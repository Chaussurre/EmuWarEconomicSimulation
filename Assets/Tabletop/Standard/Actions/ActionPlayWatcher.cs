using System;

namespace Tabletop.Standard
{
    public class ActionPlayWatcher : ActionWatcher<StandardCardData, ActionPlayWatcher.PlayData>
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
                manager.ActionsManager.AddActionImmediate(new ActionSummonWatcher.SummonData()
                {
                    CardID = cardInstance.Value.CardID,
                    CardData = cardInstance.Value.data,
                    CardModel = cardModel,
                });
        }
    }
}
