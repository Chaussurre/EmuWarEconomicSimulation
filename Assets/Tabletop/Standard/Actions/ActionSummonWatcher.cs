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
            public Card CardModel;
            public StandardCardData? CardData;
            //public StandardCardStack stack;
        }

        public CardStack Stack;

        protected override void Apply(SummonData actionData)
        {
            CardStack.CardPosition topPos = new()
            {
                stack = Stack,
                index = null,
            };

            if (actionData.CardID.HasValue)
            {
                if (actionData.CardData.HasValue)
                    manager.UpdateCard(actionData.CardID.Value, actionData.CardData.Value);

                manager.MoveCard(actionData.CardID.Value, topPos);
                return;
            }

            manager.CreateInstance(actionData.CardModel, topPos, actionData.CardData);
        }
    }
}