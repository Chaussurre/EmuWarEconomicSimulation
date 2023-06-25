using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Tabletop;

namespace Tabletop.Tests
{
    public class CardPoolTests
    {
        public class TestCard : Card<int> { }

        private CardPool<int> cardPool;
        private List<Card<int>> Cards;
        private TestCard hiddenCard;

        [SetUp]
        public void SetUp()
        {
            // Create a new CardPool instance
            cardPool = new CardPool<int>();

            // Create some example cards
            Cards = new();
            Cards.Add(new GameObject("Card 1").AddComponent<TestCard>());
            Cards.Add(new GameObject("Card 2").AddComponent<TestCard>());
            Cards.Add(new GameObject("Card 3").AddComponent<TestCard>());

            hiddenCard = new GameObject("Hidden Card").AddComponent<TestCard>();

            // Add the cards to the CardPool
            cardPool.Cards = Cards;
            cardPool.HiddenCardTemplate = hiddenCard;
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var card in Cards)
                GameObject.DestroyImmediate(card.gameObject);

            GameObject.DestroyImmediate(hiddenCard.gameObject);
        }

        [Test]
        public void GetCardFromInstance()
        {
            var instance = new Card<int>.CardInstance
            {
                CardModelID = 0,
                CardID = 1,
                hidden = false,
            };

            var result = cardPool.GetCard(instance);

            Assert.AreEqual(Cards[0], result);
        }

        [Test]
        public void GetCardFromID()
        {
            var result = cardPool.GetCard(1); // Index of card2 in the CardPool.Cards list

            Assert.AreEqual(Cards[1], result);
        }

        [Test]
        public void GetCardFromName()
        {
            var result = cardPool.GetCard("Card 3");

            Assert.AreEqual(Cards[2], result);
        }

        [Test]
        public void GetCardInvalidName()
        {
            var result = cardPool.GetCard("Non-existent Card");

            Assert.AreEqual(hiddenCard, result);
        }

        [Test]
        public void GetCardIndex()
        {
            var result = cardPool.GetCardIndex(Cards[1]);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetCardIndexInvalid()
        {
            var nonExistingCard = new GameObject("Non-existing Card").AddComponent<TestCard>();

            var result = cardPool.GetCardIndex(nonExistingCard);

            Assert.AreEqual(-1, result);

            GameObject.DestroyImmediate(nonExistingCard.gameObject);
        }

        [Test]
        public void GetHiddenCardTemplate()
        {
            var result = cardPool.GetHiddenCardTemplate();

            Assert.AreEqual(hiddenCard, result);
        }
    }
}
