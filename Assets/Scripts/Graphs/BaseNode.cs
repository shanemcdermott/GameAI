using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : MonoBehaviour, IHasConnections<GameObject, GameConnection>
{
    public GameObject[] neighbors;

    public void GetConnections(out GameConnection[] connections)
    {
        connections = new GameConnection[neighbors.Length];
        for(int i = 0; i < neighbors.Length; i++)
        {
            connections[i] = new GameConnection(gameObject, neighbors[i]);
        }
    }
}
