using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "StatusAlteration", menuName = "Combat System/Status Alteration")]
    public class StatusAlteration : ScriptableObject
    {
        public StatusAlterationEffect EffectPrefab;
        public string StatusName;
    }
}
