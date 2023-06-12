using CombatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public interface IActionWatcher<TCardData> where TCardData : struct
    {
        public void Init(ActionsManager<TCardData> manager);

        public void Trigger();

        public int GetSize();
    }

    public class ActionWatcher<TCardData, TActionData> : IActionWatcher<TCardData> where TActionData : struct where TCardData : struct
    {
        protected ActionsManager<TCardData> manager;

        private Queue<TActionData> Actions = new();

        public DataWatcher<TActionData> DataWatcher;


        public void Init(ActionsManager<TCardData> manager)
        {
            this.manager = manager;
        }

        public int GetSize() => Actions.Count;

        public void AddAction(TActionData actionData)
        {
            Actions.Enqueue(actionData);
        }

        public void Trigger()
        {
            var data = Actions.Dequeue();

            data = DataWatcher.WatchData(data);

            Apply(data);
        }

        protected virtual void Apply(TActionData actionData)
        {

        }
    }
}
