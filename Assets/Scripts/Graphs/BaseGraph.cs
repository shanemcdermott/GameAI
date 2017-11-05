using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGraph : MonoBehaviour, IGraph<int>
{
    /// <summary>
    /// Nodes are only linked the the node following them.
    /// </summary>
    public GameObject[] nodes;


    public void GetConnections(int fromNode, out IConnection<int>[] connections)
    {
        BaseNode con = nodes[fromNode].GetComponent<BaseNode>();
        if (con)
        {
            GameConnection[] neighbors;
            con.GetConnections(out neighbors);
            connections = new BaseConnection<int>[neighbors.Length];
            for(int i = 0; i < neighbors.Length; i++)
            {
                int j = Array.IndexOf(nodes, neighbors[i].GetToNode());
                connections[i] = new BaseConnection<int>(fromNode, j);
            }

        }
        else
        {
            connections = new BaseConnection<int>[1];
            connections[0] = new BaseConnection<int>(fromNode, fromNode + 1);
        }
    }



    private void OnDrawGizmos()
    {
        if (nodes.Length < 2) return;
        Gizmos.color = Color.blue;
        for(int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] == null) continue;


            BaseNode bn = nodes[i].GetComponent<BaseNode>();
            if (bn != null)
            {
                foreach(GameObject go in bn.neighbors)
                    Gizmos.DrawLine(nodes[i].transform.position, go.transform.position);
            }
        }

        Gizmos.color = Color.green;
        foreach (GameObject go in nodes)
        {
            Gizmos.DrawSphere(go.transform.position, 0.25f);
        }
    }

}
