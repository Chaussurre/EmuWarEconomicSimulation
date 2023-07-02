using NUnit.Framework;
using UnityEngine;
using Tabletop;
using System.Collections.Generic;

namespace Tabletop.Tests
{

    public class CardStackTests
    {
        class TestCardStack : CardStack<int> { }

        private TestCardStack cardStack;
        private List<Card<int>.CardInstance> CardInstances;
        private Card<int>.CardInstance extraInstance;

        [SetUp]
        public void SetUp()
        {
            extraInstance = new()
            {
                CardID = 3,
                CardModelID = 3,
                data = 300,
            };

            // Create a new instance of CardStack and initialize card instances for testing
            cardStack = new GameObject().AddComponent<TestCardStack>();
            CardInstances = new()
            {
                new()
                {
                    CardModelID = 0,
                    CardID = 0,
                    data = 0
                },
                new()
                {
                    CardModelID = 1,
                    CardID = 1,
                    data = 100
                },
                new()
                {
                    CardModelID = 2,
                    CardID = 2,
                    data = 200
                }
            };
        }
        private void AddCards()
        {
            foreach (var card in CardInstances)
                cardStack.AddCard(card);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up created objects
            Object.DestroyImmediate(cardStack.gameObject);
        }

        [Test]
        public void SizeNewStack()
        {
            // Assert
            Assert.Zero(cardStack.Size);
        }

        [Test]
        public void GetCardInvalidIndex()
        {
            AddCards();

            // Act
            var card = cardStack.GetCard(3);

            // Assert
            Assert.Null(card);
        }

        [Test]
        public void GetCard()
        {
            AddCards();

            // Act
            var card = cardStack.GetCard(1);

            // Assert
            Assert.AreEqual(CardInstances[1], card);
        }

        [Test]
        public void AddNewCardInstance()
        {
            AddCards();

            // Act
            var result = cardStack.AddCard(extraInstance);

            // Assert
            Assert.True(result);
            Assert.AreEqual(4, cardStack.Size);
            Assert.AreEqual(extraInstance, cardStack.GetCard(3));
        }

        [Test]
        public void InsertCardInvalidIndex()
        {
            AddCards();

            // Act
            var result = cardStack.InsertCard(extraInstance, -1);

            // Assert
            Assert.False(result);
            Assert.AreEqual(3, cardStack.Size);
        }

        [Test]
        public void InsertCard()
        {
            AddCards();

            // Act
            var result = cardStack.InsertCard(extraInstance, 1);

            // Assert
            Assert.True(result);
            Assert.AreEqual(4, cardStack.Size);
            Assert.AreEqual(extraInstance, cardStack.GetCard(1));
        }

        [Test]
        public void GetCardPosCardIDDoesNotExist()
        {
            AddCards();

            // Act
            var cardPosition = cardStack.GetCardPos(3);

            // Assert
            Assert.Null(cardPosition);
        }

        [Test]
        public void GetCardPos()
        {
            AddCards();

            // Act
            var cardPosition = cardStack.GetCardPos(CardInstances[1].CardID);

            // Assert
            Assert.NotNull(cardPosition);
            Assert.AreEqual(1, cardPosition?.index);
            Assert.AreEqual(cardStack, cardPosition?.stack);
        }

        [Test]
        public void UpdateCardInvalidIndex()
        {
            AddCards();

            // Act
            var result = cardStack.UpdateCard(999, -1);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void UpdateCard()
        {
            AddCards();

            // Act
            var result = cardStack.UpdateCard(999, 0);

            // Assert
            Assert.True(result);
            Assert.AreEqual(999, cardStack.GetCard(0)?.data);
        }

        [Test]
        public void SetCardInvalidIndex()
        {
            AddCards();

            // Act
            var result = cardStack.SetCard(extraInstance, -1);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void SetCard()
        {
            AddCards();

            // Act
            var result = cardStack.SetCard(extraInstance, 0);

            // Assert
            Assert.True(result);
            Assert.AreEqual(extraInstance, cardStack.GetCard(0));
        }

        [Test]
        public void RemoveCardInvalidIndex()
        {
            AddCards();

            // Act
            var result = cardStack.RemoveCard(3);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void RemoveCard()
        {
            AddCards();

            // Act
            var result = cardStack.RemoveCard(1);

            // Assert
            Assert.True(result);
            Assert.AreEqual(2, cardStack.Size);
            Assert.AreEqual(CardInstances[2], cardStack.GetCard(1));
        }
    }
}