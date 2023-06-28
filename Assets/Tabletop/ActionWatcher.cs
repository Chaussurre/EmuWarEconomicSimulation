using CombatSystem;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tabletop.Tests")]
namespace Tabletop
{
    internal interface IActionWatcher<TCardData> where TCardData : struct
    {
        internal void Init(CardManager<TCardData> manager);

        internal void Trigger();

        public int GetSize();
    }

    
    public abstract class ActionWatcher<TCardData, TActionData> : MonoBehaviour, IActionWatcher<TCardData> where TActionData : struct where TCardData : struct
    {
        protected CardManager<TCardData> CardManager;

        private List<TActionData> Actions = new();

        public DataWatcher<TActionData> DataWatcher = new();

        void IActionWatcher<TCardData>.Init(CardManager<TCardData> manager)
        {
            this.CardManager = manager;
        }

        public int GetSize() => Actions.Count;

        public void AddAction(TActionData actionData)
        {
            Actions.Add(actionData);
        }

        public void AddActionImmediate(TActionData actionData)
        {
            Actions.Insert(0, actionData);
        }

        void IActionWatcher<TCardData>.Trigger()
        {
            if (Actions.Count == 0)
                return;

            var data = Actions[0];
            Actions.RemoveAt(0);

            data = DataWatcher.WatchData(data);

            Apply(data);
        }

        protected abstract void Apply(TActionData actionData);
    }
}
