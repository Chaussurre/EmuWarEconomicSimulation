using CombatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinator
{
    public class PlayerManager: MonoBehaviour
    {

        [SerializeField] int Players;

        public int PlayerCount => Players;

        public static PlayerMask CreatePlayerMask(int player) => new PlayerMask(player);

        public static PlayerMask CreatePlayerMask(params int[] players) => new PlayerMask(players);

        protected bool DoAction<ActionData, TCardData>(
            ActionData actionData,
            DataWatcher<ActionData> dataWatcher,
            CardManager<TCardData> cardManager)
            where ActionData : IActionData
            where TCardData : struct
        {
            actionData = dataWatcher.WatchDataNoReaction(actionData);

            if (actionData.IsActionAllowed())
            {
                dataWatcher.ForceReact(actionData);
                actionData.DoAction(cardManager);

                return true;
            }

            return false;
        }

        protected bool IsActionValid<ActionData>(ActionData actionData, DataWatcher<ActionData> dataWatcher) where ActionData : IActionData
        {
            actionData = dataWatcher.WatchDataNoReaction(actionData);

            return actionData.IsActionAllowed();
        }


        protected interface IActionData
        {
            public bool IsActionAllowed();

            public void DoAction<TCardData>(CardManager<TCardData> cardManager) where TCardData : struct;
        }
    }
}
