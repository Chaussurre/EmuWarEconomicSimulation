using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CombatSystem
{

    public class ResourcesManager : MonoBehaviour
    {
        public struct ResourceModification
        {
            public int originalValue;
            public int originalMax;
            public int max;
            public int originalMin;
            public int min;
            public int delta;
            public float multiplier;
            public ICombatSystemSource source;
            public List<string> tags;
        }

        [Serializable]
        public struct ResourceData
        {
            public Resource Resource;
            public int MaxValue;
            public int MinValue;
            public int Value;
            public DataWatcher<ResourceModification> DataWatcher;
        }

        public List<ResourceData> Resources;

        public void ChangeResource(Resource resource, int delta, ICombatSystemSource source, List<string> tags)
        {
            if (Resources.All(x => x.Resource != resource))
                throw new ArgumentOutOfRangeException("resource");

            int index = Resources.FindIndex(x => x.Resource == resource);

            var data = Resources[index];

            ResourceModification modification = new()
            {
                originalValue = data.Value,
                multiplier = 1f,
                originalMax = data.MaxValue,
                max = data.MaxValue,
                originalMin = data.MinValue,
                min = data.MinValue,
                delta = delta,
                source = source,
                tags = tags,
            };

            modification = data.DataWatcher.WatchData(modification);

            data.MaxValue = modification.max;
            data.MinValue = Mathf.Min(modification.max, modification.min);
            data.Value += Mathf.RoundToInt(modification.delta * modification.multiplier);
            data.Value = Mathf.Clamp(data.Value, data.MinValue, data.MaxValue);

            Resources[index] = data;
        }
    }
}
