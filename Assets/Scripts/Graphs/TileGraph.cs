using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph : MonoBehaviour, IGraph<Vector3>
{
    public bool drawGizmos = false;

    public Vector3 tileSize = new Vector3(1.0f, 0.5f, 1.0f);
    public Vector3 extents = new Vector3(20,1,20);

    private Vector3 tileExtents;


    public void Awake()
    {
        tileExtents = Vector3.Scale(tileSize, new Vector3(0.5f, 0.5f, 0.5f));
    }


    public void GetConnections(Vector3 fromNode, out IConnection<Vector3>[] connections)
    {
        List<Vector3> results = new List<Vector3>();
        Vector3 node = new Vector3(fromNode.z,fromNode.y, fromNode.z);
        for(float x = -1; x <=1; x+=1)
        {
            for(float z = -1; z <= 1; z+=1)
            {
                node.x = fromNode.x + x * tileSize.x;
                node.z = fromNode.z + z * tileSize.z;

                Collider[] hits = Physics.OverlapBox(node, tileExtents);
                if (hits.Length == 0)
                    results.Add(node);
            }

        }
        connections = new IConnection<Vector3>[results.Count];
        for(int i = 0; i < results.Count; i++)
        {
            connections[i] = new BaseConnection<Vector3>(fromNode, results[i]);
        }
        
    }



    public void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        if (tileSize.x ==0 || tileSize. y == 0 || tileSize.z == 0 || extents.x == 0 || extents.y == 0 || extents.z == 0) return;

        Vector2 min = new Vector2(transform.position.x - extents.x, transform.position.z - extents.z);
        Vector2 max = new Vector2(transform.position.x + extents.x, transform.position.z + extents.z);
        Vector3 center = transform.position;

        for (center.x = min.x; center.x <= max.x; center.x += tileSize.x)
        {
            for (center.z = min.y; center.z <= max.y; center.z += tileSize.z)
            {
                Collider[] hits = Physics.OverlapBox(center, tileExtents);
                if (hits.Length == 0)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawCube(center, tileSize);

                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(center, tileSize);
            }

        }

        Gizmos.color = Color.blue;
        Vector3 bounds = new Vector3(2,2,2);
        bounds.Scale(extents);
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
