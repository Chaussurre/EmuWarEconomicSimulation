using System;
using UnityEngine;

namespace Tabletop.Standard
{

    public class ActionCreateCardWatcher : ActionWatcher<StandardCardData, ActionCreateCardWatcher.CreateCardData>
    {
        [Serializable]
        public struct CreateCardData
        {
            public int CardModelID;
            public StandardCardStack.CardPosition Position;
        }

        protected override void Apply(CreateCardData actionData)
        {
            var card = manager.CardPool.GetCard(actionData.CardModelID);

            manager.CreateInstance(card, actionData.Position);
        }
    }
}
