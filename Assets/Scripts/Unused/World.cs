using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World _instance = null;

    public List<GameObject> objects = new List<GameObject>();

    public GameObject tileGraph;

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

    public static GameObject GetObject(int id)
    {
        return _instance.objects[id];
    }


}
