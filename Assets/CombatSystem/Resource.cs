using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Combat System/Create Resource")]
    public class Resource : ScriptableObject
    {
        public string Name;
        public Color Color;
    }
}
