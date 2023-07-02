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
            CardManager.DestroyInstance(actionData.CardID);
        }

        public static DeathData Kill(int CardID)
        {
            return new() { CardID = CardID };
        }
    }
}