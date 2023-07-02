using NUnit.Framework;
using UnityEngine;

namespace Cardinator.Tests
{
    public class ActionWatcherTests
    {
        private class TestActionWatcher : ActionWatcher<int, int>
        {
            public int LastAppliedValue;
            public bool Called;

            protected override void Apply(int actionData)
            {
                LastAppliedValue = actionData;
                Called = true;
            }
        }

        private TestActionWatcher actionWatcher;
        private IActionWatcher<int> actionWatcherInterface;

        [SetUp]
        public void SetUp()
        {
            actionWatcher = new GameObject("ActionWatcher").AddComponent<TestActionWatcher>();
            actionWatcherInterface = actionWatcher;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(actionWatcher.gameObject);
        }

        [Test]
        public void GetSizeNoActions()
        {
            // Assert that GetSize() returns 0 when there are no actions
            Assert.AreEqual(0, actionWatcher.GetSize());
        }

        [Test]
        public void AddAction()
        {
            actionWatcher.AddAction(1);

            Assert.AreEqual(1, actionWatcher.GetSize());
        }

        [Test]
        public void AddActions()
        {
            actionWatcher.AddAction(1);
            actionWatcher.AddAction(2);
            actionWatcher.AddAction(3);

            Assert.AreEqual(3, actionWatcher.GetSize());
        }

        [Test]
        public void TriggerActions ()
        {
            actionWatcher.AddAction(1);
            actionWatcher.AddAction(2);
            actionWatcher.AddAction(3);

            Assert.AreEqual(3, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(1, actionWatcher.LastAppliedValue);
            Assert.AreEqual(2, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(2, actionWatcher.LastAppliedValue);
            Assert.AreEqual(1, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(3, actionWatcher.LastAppliedValue);
            Assert.AreEqual(0, actionWatcher.GetSize());

            Assert.IsTrue(actionWatcher.Called);
        }

        [Test]
        public void AddActionImmediate()
        {
            actionWatcher.AddActionImmediate(1);
            actionWatcher.AddActionImmediate(2);
            actionWatcher.AddActionImmediate(3);


            Assert.AreEqual(3, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(3, actionWatcher.LastAppliedValue);
            Assert.AreEqual(2, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(2, actionWatcher.LastAppliedValue);
            Assert.AreEqual(1, actionWatcher.GetSize());
            actionWatcherInterface.Trigger();
            Assert.AreEqual(1, actionWatcher.LastAppliedValue);
            Assert.AreEqual(0, actionWatcher.GetSize());

            Assert.IsTrue(actionWatcher.Called);
        }

        [Test]
        public void TriggerNoActions()
        {
            actionWatcherInterface.Trigger();
            Assert.IsFalse(actionWatcher.Called);
        }
    }
}
