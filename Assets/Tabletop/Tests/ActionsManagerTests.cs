using CombatSystem;
using System.Collections.Generic;
using UnityEngine;
using Tabletop;
using NUnit.Framework;

namespace Tabletop.Tests
{

    public class ActionsManagerTests
    {
        public class SimpleActionWatcher : ActionWatcher<int, int>
        {
            public int LastAppliedValue { get; private set; } = 0;
            public bool called = false;

            protected override void Apply(int actionData)
            {
                LastAppliedValue = actionData;
                called = true;
            }
        }
        public class ComplexActionWatcher : ActionWatcher<int, ComplexActionWatcher.ComplexData>
        {
            public ActionsManager<int> actionsManager;

            public struct ComplexData
            {
                public int value1;
                public int value2;
                public bool immediate;
            }

            protected override void Apply(ComplexData actionData)
            {
                if (actionData.immediate)
                {
                    actionsManager.AddActionImmediate(actionData.value1);
                    actionsManager.AddActionImmediate(actionData.value2);
                    return;
                }

                actionsManager.AddAction(actionData.value1);
                actionsManager.AddAction(actionData.value2);
            }
        }

        private ActionsManager<int> actionsManager;
        private SimpleActionWatcher simpleWatcher;
        private ComplexActionWatcher complexWatcher;

        [SetUp]
        public void SetUp()
        {
            // Set up the dependencies and create an instance of ActionsManager
            simpleWatcher = new GameObject ("simple watcher").AddComponent<SimpleActionWatcher>();
            complexWatcher = new GameObject("complex watcher").AddComponent<ComplexActionWatcher>();
            actionsManager = new ActionsManager<int>();
            actionsManager.Init(new IActionWatcher<int>[] { simpleWatcher, complexWatcher }, null);
            complexWatcher.actionsManager = actionsManager;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(simpleWatcher.gameObject);
            GameObject.DestroyImmediate(complexWatcher.gameObject);
        }

        [Test]
        public void AddActionNoMatchingWatcher()
        {
            Assert.IsFalse(actionsManager.AddAction(42f));
        }

        [Test]
        public void AddAction()
        {
            Assert.IsTrue(actionsManager.AddAction(42));
            Assert.AreEqual(42, simpleWatcher.LastAppliedValue);
        }

        [Test]
        public void AddActionImmediateNoMatchingWatcher()
        {
            // Create an action data and add it immediately to the manager, assert that it returns false
            Assert.IsFalse(actionsManager.AddActionImmediate(42f));
        }

        [Test]
        public void AddActionImmediate()
        {
            Assert.IsTrue(actionsManager.AddActionImmediate(42));
            Assert.AreEqual(42, simpleWatcher.LastAppliedValue);
        }

        [Test]
        public void TriggerNoActions()
        {
            actionsManager.Trigger();
            Assert.IsFalse(simpleWatcher.called);
        }


        [Test]
        public void AddActionComplex()
        {
            var data = new ComplexActionWatcher.ComplexData()
            {
                immediate = false,
                value1 = 20,
                value2 = 40,
            };

            Assert.IsTrue(actionsManager.AddAction(data));
            Assert.AreEqual(40, simpleWatcher.LastAppliedValue);
        }


        [Test]
        public void AddActionImmediateComplex()
        {
            var data = new ComplexActionWatcher.ComplexData()
            {
                immediate = true,
                value1 = 20,
                value2 = 40,
            };

            Assert.IsTrue(actionsManager.AddAction(data));
            Assert.AreEqual(20, simpleWatcher.LastAppliedValue);
        }
    }
}
