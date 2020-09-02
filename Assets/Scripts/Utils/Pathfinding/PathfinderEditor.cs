﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;

[CustomEditor(typeof(Pathfinder))]
[CanEditMultipleObjects]
public class PathfinderEditor : Editor
{
    static List<string> layers;
    static string[] layerNames;

    public static LayerMask LayerMaskField(string label, LayerMask selected)
    {

        if (layers == null)
        {
            layers = new List<string>();
            layerNames = new string[4];
        }
        else
        {
            layers.Clear();
        }

        int emptyLayers = 0;
        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);

            if (layerName != "")
            {

                for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                layers.Add(layerName);
            }
            else
            {
                emptyLayers++;
            }
        }

        if (layerNames.Length != layers.Count)
        {
            layerNames = new string[layers.Count];
        }
        for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];

        selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);

        return selected;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Pathfinder myScript = (Pathfinder)target;
        if(GUILayout.Button("Learn Level"))
        {
            myScript.Learn();
        }

        myScript.layerMask = LayerMaskField("Mask", myScript.layerMask);
    }
}
#endif