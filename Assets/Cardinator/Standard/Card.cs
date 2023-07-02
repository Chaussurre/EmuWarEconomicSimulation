using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cardinator.Standard
{
    [Serializable]
    public struct CardData
    {
        public int cost;
        public int MaxHp;
        public int Hp;
        public int Attack;
        public int Tokens;
        [HideInInspector] public int Owner;
    }

    public class Card : Card<CardData>
    {
        public bool IsAUnit;

        public UnityEvent<CardManager<CardData>, CardInstance> OnPlay = new();

        public UnityEvent<CardManager<CardData>, CardInstance> OnSummon = new();

        public UnityEvent<CardManager<CardData>, CardData> OnDeath = new();

        public UnityEvent<CardManager<CardData>, CardData> OnRemoved = new();
    }

    public static class CardInstanceReaderWriter
    {
        public static void WriteCardData(this NetworkWriter writer, CardData data)
        {
            writer.WriteInt(data.cost);
            writer.WriteInt(data.MaxHp);
            writer.WriteInt(data.Hp);
            writer.WriteInt(data.Attack);
            writer.WriteInt(data.Tokens);
            writer.WriteInt(data.Owner);
        }

        public static CardData ReadCardData(this NetworkReader reader)
        {
            return new()
            {
                cost = reader.ReadInt(),
                MaxHp = reader.ReadInt(),
                Hp = reader.ReadInt(),
                Attack = reader.ReadInt(),
                Tokens = reader.ReadInt(),
                Owner = reader.ReadInt(),
            };
        }

        public static void WriteCardDataNullable(this NetworkWriter writer, CardData? dataNullable)
        {
            writer.WriteBool(dataNullable.HasValue);

            if (!dataNullable.HasValue)
                return;

            writer.Write(dataNullable.Value);
        }

        public static CardData? ReadCardDataNullable(this NetworkReader reader)
        {
            if (!reader.ReadBool())
                return null;

            return reader.Read<CardData>();
        }
    }
}
