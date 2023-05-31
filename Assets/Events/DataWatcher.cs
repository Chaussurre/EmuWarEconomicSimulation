using System;
using System.Collections.Generic;

namespace CombatSystem.Events
{
    [Serializable]
    public class DataWatcher<T>
    {
        public readonly List<Func<T, T>> Modifiers;
        public readonly List<Action<T>> Reactions;
        
        public DataWatcher(int capacity)
        {
            Modifiers = new (capacity);
            Reactions = new (capacity);
        }

        public T WatchData(T Data)
        {
            Modifiers.ForEach(f => Data = f(Data));

            Reactions.ForEach(f => f(Data));
            return Data;
        }

        public void SortModifiers(List<Func<T, T>> sorted)
        {
            Modifiers.Sort((a, b) =>
            {
                if (!sorted.Contains(a) || !sorted.Contains(b))
                    return 0;


                return sorted.IndexOf(a).CompareTo(sorted.IndexOf(b));
            });
        }

        public void SortReactions(List<Action<T>> sorted)
        {
            Reactions.Sort((a, b) =>
            {
                if (!sorted.Contains(a) || !sorted.Contains(b))
                    return 0;


                return sorted.IndexOf(a).CompareTo(sorted.IndexOf(b));
            });
        }
    }
}