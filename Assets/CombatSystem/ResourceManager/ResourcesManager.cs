using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatSystem
{
    public abstract class ResourcesManager<TResourceData, TResourceModification> : MonoBehaviour 
        where TResourceData : struct
        where TResourceModification : struct
    {

        [Serializable]
        public struct ResourceValue
        {
            public Resource Resource;
            public TResourceData Data;
            public DataWatcher<TResourceModification> DataWatcher;
        }

        public List<ResourceValue> Resources;

        public void ChangeResource(Resource resource, int delta, ICombatSystemSource source, List<string> tags)
        {
            if (Resources.All(x => x.Resource != resource))
                throw new ArgumentOutOfRangeException("resource");

            int index = Resources.FindIndex(x => x.Resource == resource);

            var ResourceValue = Resources[index];
            var data = ResourceValue.Data;

            TResourceModification modification = InitResourceModificationStruct(data, resource, delta, source, tags);

            modification = ResourceValue.DataWatcher.WatchData(modification);

            data = ApplyModification(data, modification);

            Resources[index] = new()
            {
                Resource = ResourceValue.Resource,
                Data = data,
                DataWatcher = ResourceValue.DataWatcher,
            };
        }

        protected abstract TResourceModification InitResourceModificationStruct (TResourceData Data, Resource resource, int delta, ICombatSystemSource source, List<string> tags);

        protected abstract TResourceData ApplyModification(TResourceData data, TResourceModification resourceModification);

    }
}