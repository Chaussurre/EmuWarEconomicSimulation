using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatSystem
{

    public class StatusAlterationEffect : MonoBehaviour
    {
        public StatusAlteration StatusAlteration;

        public int Stacks;
        public bool HasMax;
        public int MaxStacks;

        public bool IsActive => Stacks > 0;

        public void SetStacks(int stacks)
        {
            if (HasMax)
                Stacks = Mathf.Clamp(stacks, 0, MaxStacks);
            else
                Stacks = Mathf.Max(stacks, 0);
        }

        public void AddStacks(int delta) => SetStacks(Stacks + delta);
    }
}
