using NUnit.Framework;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem.Tests
{

    public class DataWatcherTests
    {
        [Test]
        public void NoDataTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            int result = dataWatcher.WatchData(bufferData);

            Assert.AreEqual(bufferData, result);
        }

        [Test]
        public void ModifierCalledTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            bool modifiersCalled = false;
            dataWatcher.Modifiers.AddListener(data =>
            {
                modifiersCalled = true;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(modifiersCalled);
        }

        [Test]
        public void ModifierRemovedTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            bool modifiersCalled = false;
            UnityAction<DataWatcher<int>.DataWatcherBuffer> action = data =>
            {
                modifiersCalled = true;
            };

            dataWatcher.Modifiers.AddListener(action);
            dataWatcher.RemoveModifier(action);

            dataWatcher.WatchData(bufferData);

            Assert.IsFalse(modifiersCalled);
        }

        [Test]
        public void ModifierChageBufferTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            dataWatcher.Modifiers.AddListener(data =>
            {
                data.DataBuffer += 10;
            });

            int result = dataWatcher.WatchData(bufferData);

            Assert.AreEqual(bufferData + 10, result);
        }


        [Test]
        public void SeveralModifiersChageBufferTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            dataWatcher.Modifiers.AddListener(data =>
            {
                data.DataBuffer += 10;
            });

            dataWatcher.Modifiers.AddListener(data =>
            {
                data.DataBuffer *= 2;
            });

            int result = dataWatcher.WatchData(bufferData);

            Assert.AreEqual((bufferData + 10) * 2, result);
        }

        [Test]
        public void ReactionCalledTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            bool reactionsCalled = false;
            dataWatcher.Reactions.AddListener(data =>
            {
                reactionsCalled = true;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(reactionsCalled);
        }


        [Test]
        public void ReactionRemovedTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            bool reactionCalled = false;
            UnityAction<int> action = data =>
            {
                reactionCalled = true;
            };

            dataWatcher.Reactions.AddListener(action);
            dataWatcher.RemoveReaction(action);

            dataWatcher.WatchData(bufferData);

            Assert.IsFalse(reactionCalled);
        }

        [Test]
        public void ModifiersAndReactionsCalledTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            bool modifiersCalled = false;
            bool reactionsCalled = false;
            dataWatcher.Modifiers.AddListener(data =>
            {
                modifiersCalled = true;
            });
            dataWatcher.Reactions.AddListener(data =>
            {
                reactionsCalled = true;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(modifiersCalled);
            Assert.IsTrue(reactionsCalled);
        }


        [Test]
        public void ReactionSeesModifiersChange()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            bool seeingChange = false;
            
            dataWatcher.Modifiers.AddListener(data =>
            {
                data.DataBuffer += 10;
            });
            dataWatcher.Reactions.AddListener(data =>
            {
                seeingChange = data == bufferData + 10;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(seeingChange);
        }

    }
}

