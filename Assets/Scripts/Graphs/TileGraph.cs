using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph : MonoBehaviour, IGraph<int>
{
    public bool drawGizmos = false;

    public float tileSize = 1.0f;
    public Vector3 extents = new Vector3(20,1,20);

    private List<TileBox> tiles;
   

    public void GetConnections(int fromNode, out IConnection<int>[] connections)
    {
        throw new NotImplementedException();
    }

    public void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        if (tileSize == 0 || extents.x == 0 || extents.y == 0 || extents.z == 0) return;

        Vector2 min = new Vector2(transform.position.x - extents.x, transform.position.z - extents.z);
        Vector2 max = new Vector2(transform.position.x + extents.x, transform.position.z + extents.z);
        Vector3 size = new Vector3(tileSize, tileSize, tileSize);
        Vector3 center = transform.position;
        Vector3 boxExtents = new Vector3(tileSize * 0.5f, tileSize * 0.5f, tileSize * 0.5f);
        for (center.x = min.x; center.x <= max.x; center.x += tileSize)
        {
            for (center.z = min.y; center.z <= max.y; center.z += tileSize)
            {
                Collider[] hits = Physics.OverlapBox(center, boxExtents);
                if (hits.Length == 0)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawCube(center, size);

                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(center, size);
            }

        }

        Gizmos.color = Color.blue;
        Vector3 bounds = new Vector3(2,2,2);
        bounds.Scale(extents);
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}
