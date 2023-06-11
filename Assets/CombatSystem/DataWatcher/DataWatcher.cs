using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{
    [Serializable]
    public class DataWatcher<Data>
    {
        public class DataWatcherBuffer
        {
            public Data DataBuffer;
        }

        DataWatcherBuffer Buffer = new();

        public List<Action<DataWatcherBuffer>> ModifierActions = new();
        public List<Action<Data>> ReactionActions = new();

        private List<UnityAction<DataWatcherBuffer>> ModifierUnityActions = new();
        private List<UnityAction<Data>> ReactionUnityActions = new();

        [SerializeField]
        private UnityEvent<DataWatcherBuffer> Modifiers = new();
        [SerializeField]
        private UnityEvent<Data> Reactions = new();

        public void AddModifier(Action<DataWatcherBuffer> action)
        {
            ModifierActions.Add(action);
            AddModifierToUnityEvent(action);
        }

        public void AddReaction(Action<Data> action)
        {
            ReactionActions.Add(action);
            AddReactionToUnityEvent(action);
        }

        public void RemoveModifier(Action<DataWatcherBuffer> action)
        {
            if (!ModifierActions.Contains(action))
                return;

            var index = ModifierActions.IndexOf(action);
            ModifierActions.RemoveAt(index);
            Modifiers.RemoveListener(ModifierUnityActions[index]);
            ModifierUnityActions.RemoveAt(index);
        }

        public void RemoveReaction(Action<Data> action)
        {
            if (!ReactionActions.Contains(action))
                return;

            var index = ReactionActions.IndexOf(action);
            ReactionActions.RemoveAt(index);
            Reactions.RemoveListener(ReactionUnityActions[index]);
            ReactionUnityActions.RemoveAt(index);
        }

        public void UpdateActionList()
        {
            foreach (var modif in ModifierUnityActions)
                Modifiers.RemoveListener(modif);
            ModifierUnityActions.Clear();

            foreach (var react in ReactionUnityActions)
                Reactions.RemoveListener(react);
            ReactionUnityActions.Clear();

            ModifierActions.ForEach(AddModifierToUnityEvent);
            ReactionActions.ForEach(AddReactionToUnityEvent);
        }

        public Data WatchData(Data Data)
        {
            Buffer.DataBuffer = Data;

            Modifiers.Invoke(Buffer);
            Reactions.Invoke(Buffer.DataBuffer);

            return Buffer.DataBuffer;
        }

        public void ForceReact(Data Data)
        {
            Reactions.Invoke(Data);
        }

        private void AddModifierToUnityEvent(Action<DataWatcherBuffer> action)
        {
            var UnityAction = CreateDelegate(action);
            ModifierUnityActions.Add(UnityAction);
            Modifiers.AddListener(UnityAction);
        }
        private void AddReactionToUnityEvent(Action<Data> action)
        {
            var UnityAction = CreateDelegate(action);
            ReactionUnityActions.Add(UnityAction);
            Reactions.AddListener(UnityAction);
        }

        private UnityAction<T> CreateDelegate<T>(Action<T> action)
        {
            return Delegate.CreateDelegate(typeof(UnityAction<T>), action.Target, action.Method) as UnityAction<T>;
        }
    }
}
