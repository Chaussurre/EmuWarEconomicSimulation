using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Tabletop.Tests
{
    public class CardPoolTests
    {
        class TestCard : Card<int> { }
        class TestCardPool : CardPool<int> { }

        private TestCardPool cardPool;
        private List<TestCard> cards;

        [SetUp]
        public void Setup()
        {
            // Create a CardPool and populate it with some cards
            cardPool = new GameObject().AddComponent<TestCardPool>();
            cards = new List<TestCard>();

            for (int i = 0; i < 5; i++)
            {
                var card = new GameObject().AddComponent<TestCard>();
                card.name = $"Card{i}";
                card.DefaultData = i;
                cardPool.Cards.Add(card);
                cards.Add(card);
            }

            cardPool.HiddenCardTemplate = new GameObject().AddComponent<TestCard>();
        }

        [Test]
        public void ReturnsCorrectCard()
        {
            // Arrange
            var cardInstance = new TestCard.CardInstance { CardModelID = 2 };

            // Act
            var result = cardPool.GetCard(cardInstance);

            // Assert
            Assert.AreEqual(cards[2], result);
        }

        [Test]
        public void ReturnsHiddenCardTemplate()
        {
            // Arrange
            var cardInstance = new TestCard.CardInstance { CardModelID = -1 };

            // Act
            var result = cardPool.GetCard(cardInstance);

            // Assert
            Assert.AreEqual(cardPool.HiddenCardTemplate, result);
        }

        [Test]
        public void CreatesInstanceWithDefaultData()
        {
            // Arrange
            var card = cards[1];

            // Act
            var result = cardPool.CreateInstance(card);

            // Assert
            Assert.AreEqual(card.DefaultData, result.data);
        }

        [Test]
        public void CreatesInstanceWithGivenData()
        {
            // Arrange
            var card = cards[2];
            var data = 42;

            // Act
            var result = cardPool.CreateInstance(card, data);

            // Assert
            Assert.AreEqual(data, result.data);
        }

        [Test]
        public void CreatesInstanceFromNameWithDefaultData()
        {
            // Arrange
            var cardName = "Card3";
            var expectedIndex = 3;

            // Act
            var result = cardPool.CreateInstance(cardName);

            // Assert
            Assert.AreEqual(expectedIndex, result.CardModelID);
            Assert.AreEqual(cards[expectedIndex].DefaultData, result.data);
        }

        [Test]
        public void CreatesInstanceFromNameWithGivenData()
        {
            // Arrange
            var cardName = "Card4";
            var expectedIndex = 4;
            var data = 24;

            // Act
            var result = cardPool.CreateInstance(cardName, data);

            // Assert
            Assert.AreEqual(expectedIndex, result.CardModelID);
            Assert.AreEqual(data, result.data);
        }
    }
}