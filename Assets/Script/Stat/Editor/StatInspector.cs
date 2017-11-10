using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Useless mais putin !!!!

[CustomEditor(typeof(Stat))]
public class StatInspector : Editor
{

    SerializedProperty baseStat;
    SerializedProperty stat;
    float lastValue;
    public void OnEnable()
    {
        baseStat = serializedObject.FindProperty("BaseStat");
        stat = serializedObject.FindProperty("this");
        lastValue = baseStat.floatValue;
        Debug.Log("Useless editor *!!!!!!!!!!");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    //public overrid()
    //{
    //    serializedObject.Update();
    //    EditorGUILayout.Slider(baseStat, 1, 100, new GUIContent("BaseStat"));
    //    serializedObject.ApplyModifiedProperties();
    //}
}
