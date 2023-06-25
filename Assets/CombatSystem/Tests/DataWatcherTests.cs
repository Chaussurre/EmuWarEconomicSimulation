using NUnit.Framework;
using System;

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
            dataWatcher.AddModifier(data =>
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
            Action<DataWatcher<int>.DataWatcherBuffer> action = data =>
            {
                modifiersCalled = true;
            };

            dataWatcher.AddModifier(action);
            dataWatcher.RemoveModifier(action);

            dataWatcher.WatchData(bufferData);

            Assert.IsFalse(modifiersCalled);
        }

        [Test]
        public void ModifierChageBufferTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            dataWatcher.AddModifier(data =>
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
            

            dataWatcher.AddModifier(data =>
            {
                data.DataBuffer += 10;
            });

            dataWatcher.AddModifier(data =>
            {
                data.DataBuffer *= 2;
            });

            int result = dataWatcher.WatchData(bufferData);

            Assert.AreEqual((bufferData + 10) * 2, result);
        }

        [Test]
        public void ReorderModifiersTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            dataWatcher.AddModifier(data =>
            {
                data.DataBuffer += 10;
            });

            dataWatcher.AddModifier(data =>
            {
                data.DataBuffer *= 2;
            });

            dataWatcher.ModifierActions.Reverse();
            dataWatcher.UpdateActionList();

            int result = dataWatcher.WatchData(bufferData);

            Assert.AreEqual(bufferData * 2 + 10, result);
        }

        [Test]
        public void ReactionCalledTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            

            bool reactionsCalled = false;
            dataWatcher.AddReaction(data =>
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
            Action<int> action = data =>
            {
                reactionCalled = true;
            };

            dataWatcher.AddReaction(action);
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
            dataWatcher.AddModifier(data =>
            {
                modifiersCalled = true;
            });
            dataWatcher.AddReaction(data =>
            {
                reactionsCalled = true;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(modifiersCalled);
            Assert.IsTrue(reactionsCalled);
        }
        [Test]
        public void ReorderReactionsTest()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;

            bool firstReaction = false;
            bool secondReaction = false;

            dataWatcher.AddReaction(data =>
            {
                firstReaction = secondReaction;
            });

            dataWatcher.AddReaction(data =>
            {
                secondReaction = true;
            });

            dataWatcher.ReactionActions.Reverse();
            dataWatcher.UpdateActionList();

            int result = dataWatcher.WatchData(bufferData);

            Assert.IsTrue(firstReaction);
            Assert.IsTrue(secondReaction);
        }


        [Test]
        public void ReactionSeesModifiersChange()
        {
            var dataWatcher = new DataWatcher<int>();
            int bufferData = 42;
            bool seeingChange = false;
            
            dataWatcher.AddModifier(data =>
            {
                data.DataBuffer += 10;
            });
            dataWatcher.AddReaction(data =>
            {
                seeingChange = data == bufferData + 10;
            });

            dataWatcher.WatchData(bufferData);

            Assert.IsTrue(seeingChange);
        }

    }
}

