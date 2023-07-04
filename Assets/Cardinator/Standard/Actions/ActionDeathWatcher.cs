namespace Cardinator.Standard
{
    public class ActionDeathWatcher : ActionWatcher<CardData, ActionDeathWatcher.DeathData>
    {
        public struct DeathData
        {
            public int CardID;
        }

        protected override void Apply(DeathData actionData)
        {
            var instance = CardManager.GetCardInstance(actionData.CardID);
            if (!instance.HasValue)
                return;

            var cardModel = CardManager.CardPool.GetCard(instance.Value) as Card;
            cardModel.OnDeath?.Invoke(CardManager, instance.Value);
            cardModel.OnRemoved?.Invoke(CardManager, instance.Value);

            CardManager.DestroyInstance(actionData.CardID);
        }

        public static DeathData Kill(int CardID)
        {
            return new() { CardID = CardID };
        }
    }
}