using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GraphToTexture))]
public class GraphToTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GraphToTexture _target = (GraphToTexture)target;
        if(GUILayout.Button("Create Texture"))
        {
            _target.MakeTexture();
        }
        if(GUILayout.Button("Save Texture"))
        {
            _target.SaveTexture();
        }
    }

}
