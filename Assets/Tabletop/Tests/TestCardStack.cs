using Mirror;

namespace Tabletop.Tests
{

    public class TestCardStack : CardStack<int>
    {
        public bool RPCDataCalled = false;

        protected override void DataChangeReact(CardStackDataChange dataChange)
        {
            RPCDataChangeReact(dataChange);
        }

        [ClientRpc]
        private void RPCDataChangeReact(CardStackDataChange dataChange)
        {
            if (isServer)
                return;

            RPCDataCalled = true;
            DataWatcher.ForceReact(dataChange);
        }
    }

    public static class TestCardStackDataReaderWriter
    {
        public static void WriteCardStackDataChange(this NetworkWriter writer, TestCardStack.CardStackDataChange dataChange)
        {
            writer.WriteInt((int)dataChange.Change);
            writer.WriteInt(dataChange.CardChangedIndex);
            writer.Write(dataChange.OldCard);
            writer.Write(dataChange.NewCard);
        }

        public static TestCardStack.CardStackDataChange ReadCardStackDataChange(this NetworkReader reader)
        {
            return new()
            {
                Change = (TestCardStack.CardStackDataChange.ChangeType)reader.ReadInt(),
                CardChangedIndex = reader.ReadInt(),
                OldCard = reader.Read<TestCard.CardInstance>(),
                NewCard = reader.Read<TestCard.CardInstance>(),
            };
        }
        public static void WriteCardInstance(this NetworkWriter writer, TestCard.CardInstance card)
        {
            writer.WriteInt(card.CardID);
            writer.WriteBool(card.hidden);
            writer.WriteInt(card.data);
        }

        public static TestCard.CardInstance ReadCardInstance(this NetworkReader reader)
        {
            return new()
            {
                CardID = reader.ReadInt(),
                hidden = reader.ReadBool(),
                data = reader.ReadInt(),
            };
        }
    }
}
