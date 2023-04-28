using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Walker
{
    public class CharacterBehaviors : MonoBehaviour
    {
#if UNITY_EDITOR

        public List<CharacterInputData.InputEntry.InputEntrySignature> Requirements = new();
#endif

    }
}
