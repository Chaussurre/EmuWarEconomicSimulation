using Mirror;

namespace Tabletop
{
    public class TestCard : Card<int>
    {

    }

    public static class TestCardInstanceReaderWriter
    {
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
