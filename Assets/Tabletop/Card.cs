using Mirror;
using System;
using UnityEngine;

namespace Tabletop
{
    public class Card<TCardData> : MonoBehaviour where TCardData : struct
    {
        [Serializable]
        public struct CardInstance
        {
            public int CardModelID;
            public int CardID;
            public bool hidden;
            public NetworkIdentity owner;
            public TCardData data;
        }

        [SerializeField] private CardVisual<TCardData> VisualPrefab;

        public TCardData DefaultData;

        public CardVisual<TCardData> CreateVisual(CardInstance Card, CardPool<TCardData> cardPool)
        {
            var visual = Instantiate(VisualPrefab);
            visual.InitID(Card.CardModelID, cardPool);
            visual.UpdateData(Card);
            return visual;
        }
    }


    public static class NetworkCardInstanceReaderWriter
    {
        public static void WriteCardInstance<TCardData>(this NetworkWriter writer, Card<TCardData>.CardInstance card) where TCardData : struct
        {
            writer.WriteInt(card.CardModelID);
            writer.WriteBool(card.hidden);
            writer.WriteNetworkIdentity(card.owner);
            writer.Write(card.data);
        }

        public static Card<TCardData>.CardInstance ReadCardInstance<TCardData>(this NetworkReader reader) where TCardData : struct
        {
            return new()
            {
                CardModelID = reader.ReadInt(),
                hidden = reader.ReadBool(),
                owner = reader.ReadNetworkIdentity(),
                data = reader.Read<TCardData>(),
            };
        }
    }
}