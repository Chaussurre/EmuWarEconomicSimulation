using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CombatSystem
{
    [Serializable]
    public class SortableDataWatcher<Data> : IDataWatcher<Data>
    {
        IDataWatcher<Data>.DataWatcherBuffer Buffer = new();

        public readonly List<UnityAction<IDataWatcher<Data>.DataWatcherBuffer>> Modifiers = new();
        public readonly List<UnityAction<Data>> Reactions = new();

        public void AddModifier(UnityAction<IDataWatcher<Data>.DataWatcherBuffer> action)
        {
            Modifiers.Add(action);
        }

        public void AddReaction(UnityAction<Data> action)
        {
            Reactions.Add(action);
        }

        public void RemoveModifier(UnityAction<IDataWatcher<Data>.DataWatcherBuffer> action)
        {
            Modifiers.Remove(action);
        }

        public void RemoveReaction(UnityAction<Data> action)
        {
            Reactions.Remove(action);
        }

        public Data WatchData(Data Data)
        {
            Buffer.DataBuffer = Data;

            Modifiers.ForEach(f => f(Buffer));
            Reactions.ForEach(f => f(Buffer.DataBuffer));

            return Buffer.DataBuffer;
        }
    }
}
