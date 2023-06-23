using System;
using Mirror;
namespace Tabletop.Standard
{

    public class ActionSummonWatcher : ActionWatcher<StandardCardData, ActionSummonWatcher.SummonData>
    {
        [Serializable]
        public struct SummonData
        {
            public int? CardID;
            public StandardCard CardModel;
            public StandardCardData? CardData;
            public StandardCardStack stack;
        }

        protected override void Apply(SummonData actionData)
        {
            StandardCardStack.CardPosition topPos = new()
            {
                stack = actionData.stack,
                index = null,
            };

            if (actionData.CardID.HasValue)
            {
                if (actionData.CardData.HasValue)
                    manager.CardManager.UpdateCard(actionData.CardID.Value, actionData.CardData.Value);

                manager.CardManager.MoveCard(actionData.CardID.Value, topPos);
                return;
            }

            manager.CardManager.CreateInstance(actionData.CardModel, topPos, actionData.CardData);
        }
    }
}