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
        public bool IsAUnit;

        public UnityEvent<ActionsManager<StandardCardData>, CardInstance> OnPlay = new();
    }

    public static class StandardCardInstanceReaderWriter
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

        public static void WriteCardDataNullable(this NetworkWriter writer, StandardCardData? dataNullable)
        {
            writer.WriteBool(dataNullable.HasValue);

            if (!dataNullable.HasValue)
                return;

            writer.Write(dataNullable.Value);
        }

        public static StandardCardData? ReadCardDataNullable(this NetworkReader reader)
        {
            if (!reader.ReadBool())
                return null;

            return reader.Read<StandardCardData>();
        }
    }
}
