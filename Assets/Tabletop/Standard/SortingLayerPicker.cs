﻿using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System;

namespace Tabletop.Standard
{
    [Serializable]
    public struct SortingLayerPicker
    {
        public int index;

        public int id => SortingLayer.layers[index].id;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SortingLayerPicker))]
    public class SortingLayerPickerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("index");

            var layers = SortingLayer.layers.Select(x => x.name).ToArray();
            valueProp.intValue = EditorGUI.Popup(position, label.text, valueProp.intValue, layers);
        }
    }
#endif
}