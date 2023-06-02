using System;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    [Serializable]
    public class DataWatcher<T> where T : struct
    {
        public class DataWatcherBuffer
        {
            public T DataBuffer;
        }

        DataWatcherBuffer Buffer;

        public UnityEvent<DataWatcherBuffer> Modifiers = new();
        public UnityEvent<T> Reactions = new();


        public void RemoveModifier(UnityAction<DataWatcherBuffer> action)
        {
            if (Modifiers is null)
                return;

            Modifiers.RemoveListener(action);
        }

        public void RemoveReaction(UnityAction<T> action)
        {
            if (Reactions is null)
                return;

            Reactions.RemoveListener(action);
        }

        private void SetBuffer(T Data)
        {
            if (Buffer is null)
                Buffer = new();

            Buffer.DataBuffer = Data;
        }

        public T WatchData(T Data)
        {
            SetBuffer(Data);

            var tmpBuffer = Buffer;
            Modifiers?.Invoke(tmpBuffer);

            Reactions?.Invoke(tmpBuffer.DataBuffer);

            return tmpBuffer.DataBuffer;
        }
    }
}