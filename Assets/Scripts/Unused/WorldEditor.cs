using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        World world = (World)target;

        /*
        if (GUILayout.Button("Subdivide"))
        {
            world.Subdivide();
        }
        if(GUILayout.Button("Clear Tiles"))
        {
            world.DestroyTiles();
        }
        */
    }
}
