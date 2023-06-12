using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class ActionsManager<TCardData> : MonoBehaviour where TCardData : struct
    {
        public CardPool<TCardData> CardPool;

        public List<IActionWatcher<TCardData>> ActionWatchers = new();

        Queue<IActionWatcher<TCardData>> actions = new();

        private void Awake()
        {
            foreach (var action in ActionWatchers)
                action.Init(this);
        }

        public bool AddAction<TActionData>(TActionData actionData) where TActionData : struct
        {
            foreach(var action in ActionWatchers)
                if (action is ActionWatcher<TCardData, TActionData> watcher)
                {
                    watcher.AddAction(actionData);
                    actions.Enqueue(watcher);
                    return true;
                }

            return false;
        }

        public void Trigger()
        {
            if (actions.Count == 0)
                return;

            var watcher = actions.Dequeue();
            watcher.Trigger();
        }

        private void Update()
        {
            if (actions.Count == 0)
                return;

            while (actions.Count > 0)
                Trigger();
        }
    }
}
