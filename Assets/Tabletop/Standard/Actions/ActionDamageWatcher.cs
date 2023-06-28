using System;
using UnityEngine;

namespace Tabletop.Standard
{

    public class ActionDamageWatcher : ActionWatcher<CardData, ActionDamageWatcher.DamageData>
    {
        [Serializable]
        public struct DamageData
        {
            public int CardID;
            public int? SourceID;
            public bool dealDamage;
            public int damages;

        }
        protected override void Apply(DamageData actionData)
        {
            if (!actionData.dealDamage)
                return;

            var card = manager.GetCardInstance(actionData.CardID);
            if (!card.HasValue)
                return;

            var cardData = card.Value.data;
            
            cardData.Hp -= Mathf.Min(actionData.damages, cardData.Hp);

            manager.UpdateCard(actionData.CardID, cardData);
        }

        public static DamageData DealDamage(int TargetID, int damage, int? SourceID = null)
        {
            return new()
            {
                CardID = TargetID,
                SourceID = SourceID,
                dealDamage = true,
                damages = damage,
            };
        }
    }
}
