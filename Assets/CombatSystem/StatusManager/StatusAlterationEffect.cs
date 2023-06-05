using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    public class StatusAlterationEffect : MonoBehaviour
    {
        public StatusAlteration StatusAlteration;
        public StatusManager StatusManager;

        public bool AllowStacking;
        public int Stacks;
        public bool HasMax;
        public int MaxStacks;

        public bool IsActive => Stacks > 0;

        public struct StatusStackChange
        {
            public int Stacks;
            public int? MaxStacks;
            public int Delta;
            public bool AllowStacking;
            public ICombatSystemSource Source;
        }

        public UnityEventDataWatcher<StatusStackChange> DataWatcher = new();

        public void ChangeStacks(int delta, ICombatSystemSource source)
        {
            StatusStackChange data = new()
            {
                Stacks = Stacks,
                MaxStacks = HasMax ? MaxStacks : null,
                Delta = delta,
                AllowStacking = AllowStacking,
                Source = source,
            };

            data = DataWatcher.WatchData(data);

            Stacks += data.Delta;
        }
    }
}
