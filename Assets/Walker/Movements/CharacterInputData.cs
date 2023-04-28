using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Walker
{
    public class CharacterInputData : MonoBehaviour
    {

        [Serializable]

        public struct InputEntry
        {
            [Serializable]

            public struct InputEntrySignature
            {
                public string name;
                public InputType Type;
            }

            public enum InputType
            {
                Float, Int, Bool, Trigger, Vector
            }

            public InputEntrySignature signature;
            [HideInInspector] public float floatValue;
            [HideInInspector] public int intValue;
            [HideInInspector] public bool boolValue;
            [HideInInspector] public Vector3 VectorValue;
        }

        [SerializeField] List<InputEntry> Entries;

        InputEntry GetEntry(string name)
        {
            var index = Entries.FindIndex(x => x.signature.name == name);
            if(index < 0)
                throw new ArgumentException("There is no input data with that name");
            return Entries[index];
        }

        #region getters
        public float GetFloat(string name)
        {
            var entry = GetEntry(name);
            if (entry.signature.Type != InputEntry.InputType.Float)
                throw new ArgumentException("This input data is not a float");
            return entry.floatValue;
        }

        public int GetInt(string name)
        {
            var entry = GetEntry(name);
            if (entry.signature.Type != InputEntry.InputType.Int)
                throw new ArgumentException("This input data is not an int");
            return entry.intValue;
        }

        public bool GetBool(string name)
        {
            var entry = GetEntry(name);
            if (entry.signature.Type != InputEntry.InputType.Bool)
                throw new ArgumentException("This input data is not a boolean");
            return entry.boolValue;
        }

        public bool GetTrigger(string name)
        {
            var entry = GetEntry(name);
            if (entry.signature.Type != InputEntry.InputType.Trigger)
                throw new ArgumentException("This input data is not a trigger");
            return entry.boolValue;
        }

        public Vector3 GetVector(string name)
        {
            var entry = GetEntry(name);
            if (entry.signature.Type != InputEntry.InputType.Vector)
                throw new ArgumentException("This input data is not a vector");
            return entry.VectorValue;
        }
        #endregion
    }
}
