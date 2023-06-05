using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    [Serializable]
    public class UnityEventDataWatcher<Data> : IDataWatcher<Data> where Data : struct
    {
        IDataWatcher<Data>.DataWatcherBuffer Buffer = new();

        [SerializeField] private UnityEvent<IDataWatcher<Data>.DataWatcherBuffer> Modifiers = new();
        [SerializeField] private UnityEvent<Data> Reactions = new();


        public void AddModifier(UnityAction<IDataWatcher<Data>.DataWatcherBuffer> action)
        {
            Modifiers.AddListener(action);
        }

        public void AddReaction(UnityAction<Data> action)
        {
            Reactions.AddListener(action);
        }
        public void RemoveModifier(UnityAction<IDataWatcher<Data>.DataWatcherBuffer> action)
        {
            Modifiers.RemoveListener(action);
        }

        public void RemoveReaction(UnityAction<Data> action)
        {
            Reactions.RemoveListener(action);
        }

        public Data WatchData(Data Data)
        {
            Buffer.DataBuffer = Data;

            Modifiers?.Invoke(Buffer);
            Reactions?.Invoke(Buffer.DataBuffer);

            return Buffer.DataBuffer;
        }
    }
}