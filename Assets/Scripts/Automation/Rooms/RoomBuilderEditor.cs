using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomBuilder))]
public class RoomBuilderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        RoomBuilder builder = (RoomBuilder)target;
        if(GUILayout.Button("Build Walls"))
        {
            builder.BuildWalls();
        }
        if(GUILayout.Button("Build Room"))
        {
            builder.BuildRoom();
        }
        if(GUILayout.Button("Clear"))
        {
            builder.ClearWalls();
            builder.ClearCorners();
        }
    }
}
