using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeBuilder))]
public class MazeBuilderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        MazeBuilder builder = (MazeBuilder)target;
        if (GUILayout.Button("Build Maze"))
        {
            builder.BuildMaze();
        }
        if (GUILayout.Button("Clear"))
        {
            builder.ClearMaze();
        }
    }
}