using NUnit.Framework;
using UnityEngine;

namespace Cardinator.Tests
{
    public class CardManagerTests
    {
        class  TestCard : Card<int> { }

        class TestCardManager : CardManager<int> { }

        class TestCardStack : CardStack<int> { }

        private GameObject gameObject;
        private CardManager<int> cardManager;
        private TestCard Card1;
        private TestCard Card2;

        private TestCardStack Stack1;
        private TestCardStack Stack2;

        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();

            Card1 = new GameObject("card 1").AddComponent<TestCard>();
            Card1.transform.parent = gameObject.transform;
            Card1.DefaultData = 42;

            Card2 = new GameObject("card 2").AddComponent<TestCard>();
            Card2.transform.parent = gameObject.transform;
            Card2.DefaultData = 666;

            cardManager = gameObject.AddComponent<TestCardManager>();
            cardManager.CardPool.Cards = new() { Card1, Card2 };

            Stack1 = new GameObject("Stack 1").AddComponent<TestCardStack>();
            Stack2 = new GameObject("Stack 2").AddComponent<TestCardStack>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);

            Object.DestroyImmediate(Stack1.gameObject);
            Object.DestroyImmediate(Stack2.gameObject);
        }

        [Test]
        public void CreateInstanceCardNotInPool()
        {
            var otherCard = new GameObject("other card").AddComponent<TestCard>();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => cardManager.CreateInstance(otherCard, new()));

            GameObject.DestroyImmediate(otherCard.gameObject);
        }

        [Test]
        public void CreateInstanceDefaultData()
        {
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            Assert.IsNotNull(cardInstance);
            Assert.AreEqual(Card1.DefaultData, cardInstance.Value.data);
        }

        [Test]
        public void CreateInstance()
        {
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 }, 14);

            Assert.IsNotNull(cardInstance);
            Assert.AreEqual(14, cardInstance.Value.data);
        }


        [Test]
        public void GetCardPos()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Act
            var result = cardManager.GetCardPos(cardInstance.Value.CardID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Stack1, result?.stack);
            Assert.AreEqual(cardInstance.Value.CardID, result?.GetCard()?.CardID);
        }

        [Test]
        public void GetCardPosInvalidID()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Assert
            Assert.IsNull(cardManager.GetCardPos(cardInstance.Value.CardID + 1));
        }

        [Test]
        public void GetCardInstance()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Act
            var result = cardManager.GetCardInstance(cardInstance.Value.CardID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cardInstance.Value, result.Value);
        }

        [Test]
        public void GetCardInstanceInvalidID()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });
            // Assert
            Assert.IsNull(cardManager.GetCardInstance(cardInstance.Value.CardID + 1));
        }

        [Test]
        public void DestroyInstanceCardIDDoesNotExist()
        {
            Assert.IsFalse(cardManager.DestroyInstance(1));
        }

        [Test]
        public void DestroyInstance()
        {
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            Assert.IsTrue(cardManager.DestroyInstance(cardInstance.Value.CardID));
            Assert.IsNull(cardManager.GetCardInstance(cardInstance.Value.CardID));
        }

        [Test]
        public void MoveCard()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new(Stack1));
            var destinationPosition = new CardStack<int>.CardPosition(Stack2);

            // Assert
            Assert.IsTrue(cardManager.MoveCard(cardInstance.Value.CardID, destinationPosition));
            Assert.AreEqual(Stack2, cardManager.GetCardPos(cardInstance.Value.CardID)?.stack);
        }

        [Test]
        public void MoveCardInvalid()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });
            var destinationPosition = new CardStack<int>.CardPosition() { stack = Stack2, index = 14 };

            // Assert
            Assert.IsFalse(cardManager.MoveCard(cardInstance.Value.CardID, destinationPosition));
            Assert.AreEqual(Stack1, cardManager.GetCardPos(cardInstance.Value.CardID)?.stack);
        }

        [Test]
        public void UpdateCard()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Assert
            Assert.IsTrue(cardManager.UpdateCard(cardInstance.Value.CardID, 100));
            Assert.AreEqual(100, cardManager.GetCardInstance(cardInstance.Value.CardID)?.data);
        }

        [Test]
        public void UpdateCardInvalid()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Assert
            Assert.IsFalse(cardManager.UpdateCard(cardInstance.Value.CardID + 1, 100));
            Assert.AreEqual(Card1.DefaultData, cardManager.GetCardInstance(cardInstance.Value.CardID)?.data);
        }

        [Test]
        public void TransformCardInvalidModel()
        {
            // Arrange
            var InvalidModel = new GameObject("invalid card").AddComponent<TestCard>();

            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });


            // Assert
            Assert.IsFalse(cardManager.TransformCard(cardInstance.Value.CardID, InvalidModel));

            GameObject.DestroyImmediate(InvalidModel.gameObject);
        }

        [Test]
        public void TransformCardInvalidID()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });

            // Assert
            Assert.IsFalse(cardManager.TransformCard(cardInstance.Value.CardID + 1, Card2));
        }

        [Test]
        public void TransformCard()
        {
            // Arrange
            var cardInstance = cardManager.CreateInstance(Card1, new() { stack = Stack1 });
            int Card2Index = cardManager.CardPool.GetCardIndex(Card2);

            // Assert
            Assert.IsTrue(cardManager.TransformCard(cardInstance.Value.CardID, Card2));
            Assert.AreEqual(Card2Index, cardManager.GetCardInstance(cardInstance.Value.CardID)?.CardModelID);
        }
    }
}
