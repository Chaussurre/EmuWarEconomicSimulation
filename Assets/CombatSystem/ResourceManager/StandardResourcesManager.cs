using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CombatSystem
{

    public class StandardResourcesManager : ResourcesManager<
        StandardResourcesManager.ResourceData, 
        StandardResourcesManager.ResourceModification>
    {
        public struct ResourceData
        {
            public int MaxValue;
            public int MinValue;
            public int Value;
        }

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

        protected override ResourceModification InitResourceModificationStruct(ResourceData data,
                                                                               Resource resource,
                                                                               int delta,
                                                                               ICombatSystemSource source,
                                                                               List<string> tags)
        {
            return new()
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
        }

        protected override ResourceData ApplyModification(ResourceData data, ResourceModification modification)
        {
            data.MaxValue = modification.max;
            data.MinValue = Mathf.Min(modification.max, modification.min);
            data.Value += Mathf.RoundToInt(modification.delta * modification.multiplier);
            data.Value = Mathf.Clamp(data.Value, data.MinValue, data.MaxValue);

            return data;
        }
    }
}
