using CombatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
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

        [Serializable]
        public struct PlayerMask : IEnumerable<int>
        {
            long mask;

            public PlayerMask(int player)
            {
                mask = 1L << player;
            }

            public PlayerMask(params int[] players)
            {
                mask = 0;
                foreach (var player in players)
                    mask |= 1L << player;
            }

            public static PlayerMask All => new() { mask = ~0L };

            public static PlayerMask Empty => new() { mask = 0L };

            #region operators
            public static PlayerMask operator +(PlayerMask a, PlayerMask b)
            {
                return new() { mask = a.mask | b.mask };
            }

            public static PlayerMask operator -(PlayerMask a, PlayerMask b)
            {
                return new() { mask = a.mask & ~b.mask };
            }

            public static PlayerMask operator *(PlayerMask a, PlayerMask b)
            {
                return new() { mask = a.mask & b.mask };
            }

            public static PlayerMask operator +(PlayerMask mask, int player)
            {
                return mask + new PlayerMask(player);
            }

            public static PlayerMask operator -(PlayerMask mask, int player)
            {
                return mask - new PlayerMask(player);
            }

            public static PlayerMask operator *(PlayerMask mask, int player)
            {
                return mask * new PlayerMask(player);
            }

            public static implicit operator bool(PlayerMask mask)
            {
                return mask.mask > 0;
            }
            #endregion

            #region Enumerator
            struct Enumerator : IEnumerator<int>
            {
                PlayerMask mask;
                int currentPlayer;

                public Enumerator(int current, PlayerMask mask)
                {
                    this.mask = mask;
                    currentPlayer = current;
                }

                public int Current => currentPlayer;

                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    currentPlayer++;

                    for (; currentPlayer < 64; currentPlayer++)
                        if ((mask.mask & (1L << currentPlayer)) > 0)
                            return true;

                    return false;
                }

                public void Reset()
                {
                    currentPlayer = 0;
                }
            }

            public IEnumerator<int> GetEnumerator()
            {
                return new Enumerator(0, this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(0, this);
            }
            #endregion

        }

        protected interface IActionData
        {
            public bool IsActionAllowed();

            public void DoAction<TCardData>(CardManager<TCardData> cardManager) where TCardData : struct;
        }
    }
}
