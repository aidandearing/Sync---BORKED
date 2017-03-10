using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomPropertyDrawer(typeof(SequencerGradient))]
public class SequencerGradientDrawer : PropertyDrawer
{
    AnimBool foldout;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        foldout.target = EditorGUILayout.Foldout(foldout.target, new GUIContent("Sequencer"));
        //EditorGUI.BeginProperty(position, null, property);

        if (foldout.value && foldout.faded >= 0)
        {
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //Rect rectFormat = new Rect(position.x, position.y, 30, position.height);

            EditorGUILayout.BeginFadeGroup(foldout.faded);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("format"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("delay"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("duration"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("synchronisation"));
            //EditorGUILayout.PropertyField(property.FindPropertyRelative("synchroniser"));
            //EditorGUILayout.PropertyField(property.FindPropertyRelative("callback"));
            EditorGUILayout.EndFadeGroup();
        }

        //EditorGUI.EndProperty();
    }
}
