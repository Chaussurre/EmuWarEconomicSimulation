using CombatSystem;
using NUnit.Framework;
using UnityEngine;

namespace CombatSystem.Tests
{
    public class UnityEventStatusManagerTests
    {
        private UnityEventStatusManager unityEventStatusManager;
        private StatusAlteration statusAlteration;

        [SetUp]
        public void Setup()
        {
            unityEventStatusManager = new GameObject().AddComponent<UnityEventStatusManager>();
            statusAlteration = ScriptableObject.CreateInstance<StatusAlteration>();
            statusAlteration.EffectPrefab = new GameObject().AddComponent<StatusAlterationEffect>();
        }

        [Test]
        public void AddStacks()
        {
            // Act
            unityEventStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(5, unityEventStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void AddStacksWithMax()
        {
            statusAlteration.EffectPrefab.HasMax = true;
            statusAlteration.EffectPrefab.MaxStacks = 3;

            // Act
            unityEventStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(3, unityEventStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void AddStacksWithModifier()
        {
            unityEventStatusManager.GetDataWatcher(statusAlteration).AddModifier(buffer => buffer.DataBuffer.Delta -= 1);

            // Act
            unityEventStatusManager.ChangeStacks(statusAlteration, 5, null);

            // Assert
            Assert.AreEqual(4, unityEventStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void RemoveStacks()
        {
            // Act
            unityEventStatusManager.ChangeStacks(statusAlteration, 5, null);
            unityEventStatusManager.ChangeStacks(statusAlteration, -2, null);

            // Assert
            Assert.AreEqual(3, unityEventStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void RemoveStacksNegative()
        {
            // Act
            unityEventStatusManager.ChangeStacks(statusAlteration, 5, null);
            unityEventStatusManager.ChangeStacks(statusAlteration, -78, null);

            // Assert
            Assert.AreEqual(0, unityEventStatusManager.StatusAlterations[0].StatusGameObject.Stacks);
        }

        [Test]
        public void GetDataWatcher()
        {
            // Act
            var returnedDataWatcher = unityEventStatusManager.GetDataWatcher(statusAlteration);

            // Assert
            Assert.IsNotNull(returnedDataWatcher);
            Assert.AreSame(unityEventStatusManager.StatusAlterations[0].DataWatcher, returnedDataWatcher);
        }
    }
}
