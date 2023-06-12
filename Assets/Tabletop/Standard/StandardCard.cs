using Mirror;
using System;
using UnityEngine.Events;

namespace Tabletop.Standard
{
    [Serializable]
    public struct StandardCardData
    {
        public int cost;
        public int MaxHp;
        public int Hp;
        public int Attack;
        public int Tokens;
    }

    public class StandardCard : Card<StandardCardData>
    {

        public UnityEvent<ActionsManager<StandardCardData>> OnPlay = new();

    }

    public static class TestCardInstanceReaderWriter
    {
        public static void WriteCardData(this NetworkWriter writer, StandardCardData data)
        {
            writer.WriteInt(data.cost);
            writer.WriteInt(data.MaxHp);
            writer.WriteInt(data.Hp);
            writer.WriteInt(data.Attack);
            writer.WriteInt(data.Tokens);
        }

        public static StandardCardData ReadCardData(this NetworkReader reader)
        {
            return new()
            {
                cost = reader.ReadInt(),
                MaxHp = reader.ReadInt(),
                Hp = reader.ReadInt(),
                Attack = reader.ReadInt(),
                Tokens = reader.ReadInt(),
            };
        }

        public static void WriteCardInstance(this NetworkWriter writer, StandardCard.CardInstance card)
        {
            writer.WriteInt(card.CardModelID);
            writer.WriteBool(card.hidden);
            writer.Write(card.data);
        }

        public static StandardCard.CardInstance ReadCardInstance(this NetworkReader reader)
        {
            return new()
            {
                CardModelID = reader.ReadInt(),
                hidden = reader.ReadBool(),
                data = reader.Read<StandardCardData>(),
            };
        }
    }
}
