﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileAStar))]
public class AStarEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TileAStar world = (TileAStar)target;

        if(GUILayout.Button("Initialize"))
        {
            world.Init();
        }
        if(GUILayout.Button("Find Path"))
        {
            world.FindPath();
        }
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
