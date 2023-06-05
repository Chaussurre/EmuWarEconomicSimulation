using CombatSystem;
using NUnit.Framework;
using UnityEngine;

namespace CombatSystem.Tests
{
    public class SortableStatusManagerTests
    {
        private SortableStatusManager sortableStatusManager;
        private StatusAlteration statusAlteration;

        [SetUp]
        public void Setup()
        {
            sortableStatusManager = new GameObject().AddComponent<SortableStatusManager>();
            statusAlteration = ScriptableObject.CreateInstance<StatusAlteration>();
            statusAlteration.EffectPrefab = new GameObject().AddComponent<StatusAlterationEffect>();
        }

        [Test]
        public void AddStacks()
        {
            // Act
            sortableStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(5, sortableStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void AddStacksWithMax()
        {
            statusAlteration.EffectPrefab.HasMax = true;
            statusAlteration.EffectPrefab.MaxStacks = 3;

            // Act
            sortableStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(3, sortableStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void AddStacksWithModifier()
        {
            sortableStatusManager.GetDataWatcher(statusAlteration).AddModifier(buffer => buffer.DataBuffer.Delta -= 1);

            // Act
            sortableStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(4, sortableStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void RemoveStacks()
        {
            // Act
            sortableStatusManager.ChangeStacks(statusAlteration, 5, null);
            sortableStatusManager.ChangeStacks(statusAlteration, -2, null);

            // Assert
            Assert.AreEqual(3, sortableStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void RemoveStacksNegative()
        {
            // Act
            sortableStatusManager.ChangeStacks(statusAlteration, 5, null);
            sortableStatusManager.ChangeStacks(statusAlteration, -78, null);

            // Assert
            Assert.AreEqual(0, sortableStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void GetDataWatcher()
        {
            // Act
            var returnedDataWatcher = sortableStatusManager.GetDataWatcher(statusAlteration);

            // Assert
            Assert.IsNotNull(returnedDataWatcher);
            Assert.AreSame(sortableStatusManager.StatusAlterations[0].DataWatcher, returnedDataWatcher);
        }
    }
}
