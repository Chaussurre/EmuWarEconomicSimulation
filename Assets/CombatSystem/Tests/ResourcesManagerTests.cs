using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using DataModificationBuffer = CombatSystem.DataWatcher<CombatSystem.ResourcesManager.ResourceModification>.DataWatcherBuffer;

namespace CombatSystem.Tests
{
    public class ResourcesManagerTests
    {
        private ResourcesManager manager;
        private ResourcesManager.ResourceData resourceData;

        [SetUp]
        public void SetUp()
        {
            manager = new GameObject().AddComponent<ResourcesManager>();
            resourceData = new ResourcesManager.ResourceData
            {
                Resource = ScriptableObject.CreateInstance<Resource>(),
                MaxValue = 100,
                MinValue = 0,
                Value = 50,
                DataWatcher = new()
            };
            manager.Resources = new List<ResourcesManager.ResourceData> { resourceData };
        }

        [Test]
        public void ModifiesResourceCorrectly()
        {
            // Act
            manager.ChangeResource(resourceData.Resource, -20, null, null);

            // Assert
            Assert.AreEqual(30, manager.Resources[0].Value);
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
            manager.ChangeResource(resourceData.Resource, -20, null, null);

            // Assert
            Assert.AreEqual(40, manager.Resources[0].Value);
        }

        private void Add10(DataModificationBuffer buffer)
        {
            buffer.DataBuffer.delta += 10;
        }
    }
}
