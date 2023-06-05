using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatSystem
{
    public struct StatusStackChange
    {
        public int Stacks;
        public int? MaxStacks;
        public int Delta;
        public ICombatSystemSource Source;
    }

    public abstract class StatusManager<TDataWatcher> : MonoBehaviour where TDataWatcher : IDataWatcher<StatusStackChange>
    {

        [Serializable]
        public struct StatusAlterationData
        {
            public StatusAlteration Status;
            public StatusAlterationEffect StatusGameObject;
            public TDataWatcher DataWatcher;
        }

        public List<StatusAlterationData> StatusAlterations = new();


        public void InitGameObject(int index)
        {
            var statusData = StatusAlterations[index];

            if (statusData.StatusGameObject == null)
            {
                statusData.StatusGameObject = Instantiate(statusData.Status.EffectPrefab, transform);
                StatusAlterations[index] = statusData;
            }
        }

        private void Awake()
        {
            for (int i = 0; i < StatusAlterations.Count; i++)
                InitGameObject(i);
        }

        private StatusAlterationData GetStatusData(StatusAlteration status)
        {
            if (!StatusAlterations.Any(x => x.Status == status))
            {
                StatusAlterationData statusData = new() {
                    Status = status,
                    DataWatcher = InitDataWatcher(),
                };
                StatusAlterations.Add(statusData);
                InitGameObject(StatusAlterations.Count - 1);
                return StatusAlterations[^1];
            }

            return StatusAlterations.Find(x => x.Status == status);
        }

        public void ChangeStacks(StatusAlteration status, int delta, ICombatSystemSource source)
        {
            var statusData = GetStatusData(status);

            StatusStackChange data = new()
            {
                Stacks = statusData.StatusGameObject.Stacks,
                MaxStacks = statusData.StatusGameObject.HasMax ? statusData.StatusGameObject.MaxStacks : null,
                Delta = delta,
                Source = source,
            };

            data = statusData.DataWatcher.WatchData(data);

            statusData.StatusGameObject.AddStacks(data.Delta);
        }

        public IDataWatcher<StatusStackChange> GetDataWatcher(StatusAlteration status)
        {
            var statusData = GetStatusData(status);

            return statusData.DataWatcher;
        }

        protected abstract TDataWatcher InitDataWatcher();
    }
}
