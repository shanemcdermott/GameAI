using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphToTexture : MonoBehaviour
{

    public int width = 512;
    public int height = 512;
    public Texture2D texture;
    public DelaunayTriangulation dtriangles;
    public string fileName = "DelaunayTexture.png";


    public void MakeTexture()
    {
        texture = new Texture2D(width, height);
        
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Vector2 v = new Vector2(x / 5.12f, y / 5.12f);
                texture.SetPixel(x, y, dtriangles.GetPointColor(v));
            }
        }
        texture.Apply();
    }
    
    public void SaveTexture()
    {
        // Encode texture into PNG
        byte[] bytes = texture.EncodeToPNG();
        
        // For testing purposes, also write to a file in the project folder
         File.WriteAllBytes(Application.dataPath + "/../"+fileName, bytes);
    }
}
