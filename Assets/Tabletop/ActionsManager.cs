using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tabletop.Tests")]
namespace Tabletop
{
    public class ActionsManager<TCardData> where TCardData : struct
    {
        [SerializeField]
        private List<IActionWatcher<TCardData>> ActionWatchers = new();

        List<IActionWatcher<TCardData>> actions = new();

        bool resolving = false;

        internal ActionsManager(IActionWatcher<TCardData>[] watchers, CardManager<TCardData> CardManager)
        {
            foreach (var watcher in watchers)
                ActionWatchers.Add(watcher);

            foreach (var action in ActionWatchers)
                action.Init(CardManager);
        }

        public bool AddAction<TActionData>(TActionData actionData) where TActionData : struct
        {
            foreach (var watcher in ActionWatchers)
                if (AddActionToWatcher(watcher, actionData))
                    return true;

            return false;
        }

        public bool AddActionImmediate<TActionData>(TActionData actionData) where TActionData : struct
        {
            foreach (var watcher in ActionWatchers)
                if (AddActionToWatcher(watcher, actionData, Immediate: true))
                    return true;

            return false;
        }

        private bool AddActionToWatcher<TActionData>(IActionWatcher<TCardData> watcher, TActionData actionData, bool Immediate = false) where TActionData : struct
        {
            if (watcher is not ActionWatcher<TCardData, TActionData> matchingWatcher)
                return false;

            if(Immediate)
            {
                matchingWatcher.AddActionImmediate(actionData);
                actions.Insert(0, watcher);

                Resolve();
                return true;
            }

            matchingWatcher.AddAction(actionData);
            actions.Add(watcher);

            Resolve();
            return true;
        }

        public void Trigger()
        {
            if (actions.Count == 0)
                return;

            var watcher = actions[0];
            actions.RemoveAt(0);
            watcher.Trigger();
        }

        private void Resolve()
        {
            if (resolving)
                return;

            resolving = true;

            while (actions.Count > 0)
                Trigger();

            resolving = false;
        }
    }
}
