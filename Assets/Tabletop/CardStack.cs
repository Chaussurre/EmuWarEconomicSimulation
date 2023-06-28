using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[assembly: InternalsVisibleTo("Tabletop.Tests")]
namespace Tabletop
{
    public class CardStack<TCardData> : MonoBehaviour, IEnumerable<Card<TCardData>.CardInstance> where TCardData : struct
    {
        public struct CardPosition
        {
            public CardStack<TCardData> stack;
            public int? index;

            public Card<TCardData>.CardInstance? GetCard()
            {
                return stack.GetCard(index ?? stack.Size - 1);
            }

            public bool CanInsert()
            {
                if (!stack)
                    return false;

                return stack.Size >= (index ?? 0);
            }

            internal bool InsertCard(Card<TCardData>.CardInstance cardInstance)
            {
                return stack.InsertCard(cardInstance, index ?? stack.Size);
            }

            internal bool UpdateCard(TCardData data)
            {
                return stack.UpdateCard(data, index ?? stack.Size - 1);
            }

            internal bool RemoveCard()
            {
                return stack.RemoveCard(index ?? stack.Size - 1);
            }

            internal bool SetCard(Card<TCardData>.CardInstance cardInstance)
            {
                return stack.SetCard(cardInstance, index ?? stack.Size - 1);
            }
        }

        List<Card<TCardData>.CardInstance> Cards = new();

        public int Size => Cards.Count;

        public Card<TCardData>.CardInstance? GetCard(int index)
        {
            if (index < 0 || index >= Size)
                return null;

            return Cards[index];
        }

        public bool Contains(int CardID)
        {
            foreach(var card in this)
                if (card.CardID == CardID)
                    return true;

            return false;
        }

        public IEnumerator<Card<TCardData>.CardInstance> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        internal void Clear()
        {
            Cards.Clear();
        }

        internal bool AddCard(Card<TCardData>.CardInstance card)
        {
            return InsertCard(card, Cards.Count);
        }

        internal bool InsertCard(Card<TCardData>.CardInstance card, int index)
        {
            if (index < 0 || index > Cards.Count)
                return false;

            Cards.Insert(index, card);
            return true;
        }

        internal bool SetCard(Card<TCardData>.CardInstance card, int index)
        {
            if (index < 0 || index > Cards.Count - 1)
                return false;

            Cards[index] = card;
            return true;
        }

        internal CardPosition? GetCardPos(int cardID)
        {
            for (int i = 0; i < Cards.Count; i++)
                if (Cards[i].CardID == cardID)
                    return new()
                    {
                        index = i,
                        stack = this,
                    };

            return null;
        }

        internal bool UpdateCard(TCardData data, int index)
        {
            if (index < 0 || index >= Cards.Count)
                return false;

            var card = Cards[index];
            card.data = data;
            Cards[index] = card;

            return true;
        }

        internal bool RemoveCard(int index)
        {
            if (index < 0 || index >= Cards.Count)
                return false;

            Cards.RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Cards.GetEnumerator();
        }
    }
}
