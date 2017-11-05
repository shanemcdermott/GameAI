using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World _instance = null;

    public List<GameObject> objects = new List<GameObject>();

    public BoxCollider bounds;

    public GameObject tileGraph;

    public Vector3 tileSize = new Vector3(10, 10, 10);


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
          
    }

    public void DestroyTiles()
    {
        TileBox[] oldTiles = tileGraph.GetComponents<TileBox>();
        for(int i = 0; i < oldTiles.Length; i++)
        {
            DestroyImmediate(oldTiles[i]);
        }
    }

    public void Subdivide()
    {
        Vector2 halfTile = new Vector2(tileSize.x * 0.5f, tileSize.z * 0.5f);
        Vector2 min = new Vector2(bounds.center.x - bounds.size.x * 0.5f, bounds.center.z - bounds.size.z * 0.5f);
        min.x += halfTile.x;
        min.y += halfTile.y;

        Vector2 max = new Vector2(bounds.center.x + bounds.size.x * 0.5f,bounds.center.z + bounds.size.z * 0.5f);
        max.x += halfTile.x;
        max.y += halfTile.y;

        Subdivide(min, max, this.tileSize);
    }

    public void Subdivide(Vector2 min, Vector2 max, Vector3 tileSize)
    {
       for(float x = min.x; x < max.x; x+=tileSize.x)
        {
            for(float z = min.y; z < max.y; z+=tileSize.z)
            {
                TileBox box = tileGraph.AddComponent<TileBox>();
                box.AddBox(new Vector3(x, 0, z), tileSize);
                
            }
        }
    }

    public static GameObject GetObject(int id)
    {
        return _instance.objects[id];
    }


}
