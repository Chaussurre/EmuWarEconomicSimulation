using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    public interface IDataWatcher<Data>
    {
        public class DataWatcherBuffer
        {
            public Data DataBuffer;
        }

        public void AddModifier(UnityAction<DataWatcherBuffer> action);

        public void AddReaction(UnityAction<Data> action);
        public void RemoveModifier(UnityAction<DataWatcherBuffer> action);

        public void RemoveReaction(UnityAction<Data> action);

        public Data WatchData(Data Data);
    }
}
