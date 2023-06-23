using Mirror;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEditor;

namespace Tabletop.Tests
{
    public class TestCardStack : CardStack<int> { }
    public class TestCard : Card<int> { }

    public class CardStackTests
    {
        private TestCardStack cardStack;
        private List<TestCard.CardInstance> cards;
        private TestCardPool cardPool;
        private TestCard cardModel;

        private NetworkManager manager;


        [SetUp]
        public void Setup()
        {
            if (manager == null)
            {
                var prefabManager = AssetDatabase.LoadAssetAtPath<NetworkManager>("Assets/Tabletop/Tests/NetworkManagerTest.prefab");
                manager = GameObject.Instantiate(prefabManager);

                var cardStackPrefab = AssetDatabase.LoadAssetAtPath<TestCardStack>("Assets/Tabletop/Tests/CardStackTest.prefab");

                manager.StartHost();
                cardStack = NetworkBehaviour.Instantiate(cardStackPrefab);
                NetworkServer.Spawn(cardStack.gameObject);

                cardPool = new GameObject("CardPool").AddComponent<TestCardPool>();
                cardPool.PositionTracker = new();
                cardStack.CardManager = cardPool;

                cardModel = new GameObject("Card Model").AddComponent<TestCard>();
                cardPool.Cards = new() { cardModel };
            }

            // Populate the card stack with some cards
            cards = new List<TestCard.CardInstance>();

            for (int i = 0; i < 5; i++)
            {
                var cardInstance = cardPool.CreateInstance(cardModel, i);
                cards.Add(cardInstance);

                // Call AddCard on the server
                cardStack.AddCard(cardInstance);
            }
        }

        [Test]
        public void AddsCardToStack()
        {
            // Arrange
            var newCard = cardPool.CreateInstance(cardModel, 5);

            // Act
            cardStack.AddCard(newCard);

            // Assert
            Assert.AreEqual(6, cardStack.Size);
            Assert.AreEqual(newCard, cardStack.GetCard(5));
            Assert.AreEqual(5, cardPool.PositionTracker[newCard.CardID].index);
        }

        [Test]
        public void InsertsCardAtGivenIndex()
        {
            // Arrange
            var newCard = cardPool.CreateInstance(cardModel, 5);
            var insertIndex = 2;

            // Act
            cardStack.InsertCard(newCard, insertIndex);

            // Assert
            Assert.AreEqual(6, cardStack.Size);
            Assert.AreEqual(newCard, cardStack.GetCard(insertIndex));
            Assert.AreEqual(2, cardPool.PositionTracker[newCard.CardID].index);
            Assert.AreEqual(3, cardPool.PositionTracker[cardStack.GetCard(insertIndex + 1).CardID].index);
        }

        [Test]
        public void ClearCards()
        {
            // Act
            cardStack.Clear();

            // Assert
            Assert.AreEqual(0, cardStack.Size);
            Assert.AreEqual(0, cardPool.PositionTracker.Count);
        }

        [Test]
        public void UpdatesCardAtGivenIndex()
        {
            // Arrange
            var updatedCard = cardPool.CreateInstance(cardModel, 13);
            var updateIndex = 2;

            // Act
            cardStack.UpdateCard(updatedCard, updateIndex);

            // Assert
            Assert.AreEqual(5, cardStack.Size);
            Assert.AreEqual(updatedCard, cardStack.GetCard(updateIndex));
            Assert.AreEqual(updateIndex, cardPool.PositionTracker[updatedCard.CardID].index);
        }

        [Test]
        public void RemovesCardAtGivenIndex()
        {
            // Arrange
            var removeIndex = 2;

            // Act
            cardStack.RemoveCard(removeIndex);

            // Assert
            Assert.AreEqual(4, cardStack.Size);
            Assert.AreNotEqual(cards[removeIndex], cardStack.GetCard(removeIndex));
            Assert.AreEqual(removeIndex - 1, cardPool.PositionTracker[cardStack.GetCard(removeIndex - 1).CardID].index);
            Assert.AreEqual(removeIndex, cardPool.PositionTracker[cardStack.GetCard(removeIndex).CardID].index);
        }

        [TearDown]
        public void TearDown()
        {
            cards.Clear();
            cardStack.Clear();
        }
    }
}