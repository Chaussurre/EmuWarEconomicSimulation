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

        public StandardCardStack PlayingField;

        protected override void Apply(PlayData actionData)
        {
            var cardInstance = manager.CardManager.GetCardInstance(actionData.CardID);

            if (!cardInstance.HasValue)
                return;

            var cardModel = manager.CardManager.CardPool.GetCard(cardInstance.Value) as StandardCard;

            cardModel.OnPlay?.Invoke(manager, cardInstance.Value);

            if (cardModel.IsAUnit)
                manager.AddAction(new ActionSummonWatcher.SummonData()
                {
                    stack = PlayingField,
                    CardID = cardInstance.Value.CardID,
                    CardData = cardInstance.Value.data,
                    CardModel = cardModel,
                });
        }
    }
}
