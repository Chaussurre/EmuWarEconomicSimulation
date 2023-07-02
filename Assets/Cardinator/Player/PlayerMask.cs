using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cardinator
{
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

        public override string ToString()
        {
            if (mask == 0)
                return "Empty";
            if (mask == ~0)
                return "All";

            StringBuilder builder = new();
            foreach(var player in this)
            {
                builder.Append(player);
                builder.Append(" ");
            }
            return builder.ToString();
        }

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

        public static PlayerMask operator !(PlayerMask mask)
        {
            return new PlayerMask() { mask = ~mask.mask };
        }
        public static PlayerMask operator -(PlayerMask mask)
        {
            return !mask;
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
                    if (mask * currentPlayer)
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
}