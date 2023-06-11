using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using DataModificationBuffer = CombatSystem.DataWatcher<CombatSystem.StandardResourcesManager.ResourceModification>.DataWatcherBuffer;

namespace CombatSystem.Tests
{
    public class StandartResourcesManagerTests
    {
        private StandardResourcesManager manager;
        private StandardResourcesManager.ResourceValue ResourceValue;

        [SetUp]
        public void SetUp()
        {
            manager = new GameObject().AddComponent<StandardResourcesManager>();
            ResourceValue = new()
            {
                Resource = ScriptableObject.CreateInstance<Resource>(),
                Data = new()
                {
                    MaxValue = 100,
                    MinValue = 0,
                    Value = 50,
                },
                DataWatcher = new(),
            };
            manager.Resources = new List<StandardResourcesManager.ResourceValue> { ResourceValue };
        }

        [Test]
        public void ModifiesResourceCorrectly()
        {
            // Act
            manager.ChangeResource(ResourceValue.Resource, -20, null, null);

            // Assert
            Assert.AreEqual(30, manager.Resources[0].Data.Value);
        }

        [Test]
        public void ThrowsExceptionForInvalidResource()
        {
            // Act and Assert
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
            {
                manager.ChangeResource(ScriptableObject.CreateInstance<Resource>(), 10, null, null);
            });
        }

        [Test]
        public void ModifiesResourceWithModifiers()
        {
            // Arrange
            manager.Resources[0].DataWatcher.AddModifier(Add10);

            // Act
            manager.ChangeResource(ResourceValue.Resource, -20, null, null);

            // Assert
            Assert.AreEqual(40, manager.Resources[0].Data.Value);
        }

        private void Add10(DataModificationBuffer buffer)
        {
            buffer.DataBuffer.delta += 10;
        }
    }
}
