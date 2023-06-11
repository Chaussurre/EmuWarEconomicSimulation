using Mirror;

namespace Tabletop
{
    public class TestCardStack : CardStack<int>
    {
        protected override void DataChangeReact(CardStackDataChange dataChange)
        {
            RPCDataChangeReact(dataChange);
        }

        [ClientRpc]
        private void RPCDataChangeReact(CardStackDataChange dataChange)
        {
            if (isServer)
                return;

            DataWatcher.ForceReact(dataChange);
        }
    }

    public static class TestCardStackDataReaderWriter
    {
        public static void WriteCardInstance(this NetworkWriter writer, TestCardStack.CardStackDataChange dataChange)
        {
            writer.WriteInt((int)dataChange.Change);
            writer.WriteInt(dataChange.CardChangedIndex);
            writer.Write(dataChange.OldCard);
            writer.Write(dataChange.NewCard);
        }

        public static TestCardStack.CardStackDataChange ReadCardInstance(this NetworkReader reader)
        {
            return new()
            {
                Change = (TestCardStack.CardStackDataChange.ChangeType) reader.ReadInt(),
                CardChangedIndex = reader.ReadInt(),
                OldCard = reader.Read<TestCard.CardInstance>(),
                NewCard = reader.Read<TestCard.CardInstance>(),
            };
        }
    }

}
