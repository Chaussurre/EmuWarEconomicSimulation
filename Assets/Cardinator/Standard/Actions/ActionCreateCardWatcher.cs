using System;
using UnityEngine;

namespace Cardinator.Standard
{

    public class ActionCreateCardWatcher : ActionWatcher<CardData, ActionCreateCardWatcher.CreateCardData>
    {
        [Serializable]
        public struct CreateCardData
        {
            public int CardModelID;
            public CardStack.CardPosition Position;
            public PlayerMask VisibleMask;
        }

        protected override void Apply(CreateCardData actionData)
        {
            var card = CardManager.CardPool.GetCard(actionData.CardModelID);

            CardManager.CreateInstance(card, actionData.Position, VisibleTo: actionData.VisibleMask);
        }

        public static CreateCardData CreateCard(int CardModelID, CardStack.CardPosition position, PlayerMask mask)
        {
            return new()
            {
                CardModelID = CardModelID,
                Position = position,
                VisibleMask = mask,
            };
        }

        public static CreateCardData CreateCard(Card card, CardStack.CardPosition position, CardManager manager, PlayerMask mask)
        {
            return new()
            {
                CardModelID = manager.CardPool.GetCardIndex(card),
                Position = position,
                VisibleMask = mask,
            };
        }
    }
}
