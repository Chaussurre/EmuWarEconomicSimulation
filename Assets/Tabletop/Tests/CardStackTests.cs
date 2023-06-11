using Mirror;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEditor;
using System;

namespace Tabletop.Tests
{
    public class TestCard : Card<int> { }
    public class TestPlayer : NetworkBehaviour { }

    public class CardStackTests
    {
        private TestCardStack cardStack;
        private List<TestCard.CardInstance> cards;

        private NetworkManager manager;


        [SetUp]
        public void Setup()
        {
            if (manager == null)
            {
                //manager = AssetDatabase.LoadAssetAtPath<NetworkManager>("Assets/Tabletop/Tests/NetworkManagerTest.prefab");
                var prefabManager = AssetDatabase.LoadAssetAtPath<NetworkManager>("Assets/Tabletop/Tests/NetworkManagerTest.prefab");
                manager = GameObject.Instantiate(prefabManager);

                var cardStackPrefab = AssetDatabase.LoadAssetAtPath<TestCardStack>("Assets/Tabletop/Tests/CardStackTest.prefab");

                manager.StartHost();
                cardStack = NetworkBehaviour.Instantiate(cardStackPrefab);
                NetworkServer.Spawn(cardStack.gameObject);
            }

            // Populate the card stack with some cards
            cards = new List<TestCard.CardInstance>();

            for (int i = 0; i < 5; i++)
            {
                var cardInstance = new TestCard.CardInstance { CardID = i, hidden = false, data = i };
                cards.Add(cardInstance);

                // Call AddCard on the server
                cardStack.AddCard(cardInstance);
            }
        }

        [Test]
        public void AddsCardToStack()
        {
            // Arrange
            var newCard = new TestCard.CardInstance { CardID = 5, hidden = false, data = 5 };

            // Act
            cardStack.AddCard(newCard);

            // Assert
            Assert.AreEqual(cards.Count + 1, cardStack.Size);
            Assert.AreEqual(newCard, cardStack.GetCard(5));
        }

        [Test]
        public void InsertsCardAtGivenIndex()
        {
            // Arrange
            var newCard = new TestCard.CardInstance { CardID = 5, hidden = false, data = 5 };
            var insertIndex = 2;

            // Act
            cardStack.InsertCard(newCard, insertIndex);

            // Assert
            Assert.AreEqual(cards.Count + 1, cardStack.Size);
            Assert.AreEqual(newCard, cardStack.GetCard(insertIndex));
        }

        [Test]
        public void ClearCards()
        {
            // Act
            cardStack.Clear();

            // Assert
            Assert.AreEqual(0, cardStack.Size);
        }

        [Test]
        public void UpdatesCardAtGivenIndex()
        {
            // Arrange
            var updatedCard = new TestCard.CardInstance { CardID = 3, hidden = true, data = 99 };
            var updateIndex = 2;

            // Act
            cardStack.UpdateCard(updatedCard, updateIndex);

            // Assert
            Assert.AreEqual(cards.Count, cardStack.Size);
            Assert.AreEqual(updatedCard, cardStack.GetCard(updateIndex));
        }

        [Test]
        public void RemovesCardAtGivenIndex()
        {
            // Arrange
            var removeIndex = 2;

            // Act
            cardStack.RemoveCard(removeIndex);

            // Assert
            Assert.AreEqual(cards.Count - 1, cardStack.Size);
            Assert.AreNotEqual(cards[removeIndex], cardStack.GetCard(removeIndex));
        }

        [TearDown]
        public void TearDown()
        {
            cards.Clear();
            cardStack.Clear();
        }
    }
}