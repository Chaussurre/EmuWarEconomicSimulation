using System;
using Mirror;
namespace Tabletop.Standard
{

    public class ActionSummonWatcher : ActionWatcher<CardData, ActionSummonWatcher.SummonData>
    {
        [Serializable]
        public struct SummonData
        {
            public int? CardID;
            public Card CardModel;
            public CardData? CardData;
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

        public static SummonData Summon(Card CardModel, CardData? cardData)
        {
            return new()
            {
                CardID = null,
                CardModel = CardModel,
                CardData = cardData,
            };
        }

        public static SummonData Summon(int CardID, CardData? cardData)
        {
            return new()
            {
                CardID = CardID,
                CardModel = null,
                CardData = cardData,
            };
        }
    }
}