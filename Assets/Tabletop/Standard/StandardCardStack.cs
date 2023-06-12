using Mirror;

namespace Tabletop.Standard
{
    public class StandardCardStack : CardStack<StandardCardData>
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
        public static void WriteCardInstance(this NetworkWriter writer, StandardCardStack.CardStackDataChange dataChange)
        {
            writer.WriteInt((int)dataChange.Change);
            writer.WriteInt(dataChange.CardChangedIndex);
            writer.Write(dataChange.OldCard);
            writer.Write(dataChange.NewCard);
        }

        public static StandardCardStack.CardStackDataChange ReadCardInstance(this NetworkReader reader)
        {
            return new()
            {
                Change = (StandardCardStack.CardStackDataChange.ChangeType) reader.ReadInt(),
                CardChangedIndex = reader.ReadInt(),
                OldCard = reader.Read<StandardCard.CardInstance>(),
                NewCard = reader.Read<StandardCard.CardInstance>(),
            };
        }
    }

}
