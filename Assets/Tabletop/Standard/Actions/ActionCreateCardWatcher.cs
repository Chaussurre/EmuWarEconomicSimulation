using System;
using UnityEngine;

namespace Tabletop.Standard
{

    public class ActionCreateCardWatcher : ActionWatcher<CardData, ActionCreateCardWatcher.CreateCardData>
    {
        [Serializable]
        public struct CreateCardData
        {
            public int CardModelID;
            public CardStack.CardPosition Position;
        }

        protected override void Apply(CreateCardData actionData)
        {
            var card = CardManager.CardPool.GetCard(actionData.CardModelID);

            CardManager.CreateInstance(card, actionData.Position);
        }

        public static CreateCardData CreateCard(int CardModelID, CardStack.CardPosition position)
        {
            return new()
            {
                CardModelID = CardModelID,
                Position = position,
            };
        }

        public static CreateCardData CreateCard(Card card, CardStack.CardPosition position, CardManager manager)
        {
            return new()
            {
                CardModelID = manager.CardPool.GetCardIndex(card),
                Position = position,
            };
        }
    }
}
