using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph : MonoBehaviour, IGraph<Vector3>
{
    public Vector3 tileSize = new Vector3(1.0f, 0.5f, 1.0f);
    public Vector3 graphSize = new Vector3(20,1,20);

    public Vector3 tileExtents;
    public Vector3 graphExtents;

    private int blockingMask;

    public void Awake()
    {
        Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
        tileExtents = Vector3.Scale(tileSize, half);
        graphExtents = Vector3.Scale(graphSize, half);
        blockingMask =  LayerMask.NameToLayer("Blocking");
    }

    public Vector2 WorldToTile(Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x / tileSize.x, worldPosition.z / tileSize.z);
    }

    public Vector3 TileToWorld(Vector2 tilePosition)
    {
        return new Vector3(tilePosition.x * tileSize.x, transform.position.y, tilePosition.y * tileSize.z);
    }

    public Vector3 WorldToTileCenter(Vector3 worldPosition)
    {
        return TileToWorld(WorldToTile(worldPosition));
    }

    private void GetConnections(Vector2 fromTile, out IConnection<Vector2>[] connections)
    {
        List<Vector2> results = new List<Vector2>();

        for (float x = -1; x <= 1; x += 1)
        {
            for (float z = -1; z <= 1; z += 1)
            {
                Vector2 tileNode = new Vector2(fromTile.x + x, fromTile.y + z);
                Vector3 worldNode = TileToWorld(tileNode);

                Collider[] hits = Physics.OverlapBox(worldNode, tileExtents);
                if (hits.Length == 0)
                    results.Add(tileNode);
            }

        }
        connections = new IConnection<Vector2>[results.Count];
        for (int i = 0; i < results.Count; i++)
        {
            connections[i] = new BaseConnection<Vector2>(fromTile, results[i]);
        }
    }

    public void GetConnections(Vector3 fromNode, out IConnection<Vector3>[] connections)
    {
        List<Vector3> results = new List<Vector3>();
        Vector2 fromTile = WorldToTile(fromNode);

        for (float x = -1; x <= 1; x += 1)
        {
            for (float z = -1; z <= 1; z += 1)
            {
                if (x == 0 && z == 0) continue;

                //Vector2 tileNode = new Vector2(fromTile.x + x, fromTile.y + z);
                //Vector3 worldNode = TileToWorld(tileNode);
                Vector3 worldNode = new Vector3(fromNode.x + (x * tileSize.x), fromNode.y, fromNode.z + (z * tileSize.z));
                Collider[] hits = Physics.OverlapBox(worldNode, tileExtents);
                if (hits.Length == 0)
                    results.Add(worldNode);
            }

        }
        
        connections = new IConnection<Vector3>[results.Count];
        for (int i = 0; i < results.Count; i++)
        {
            BaseConnection<Vector3> connection = new BaseConnection<Vector3>(fromNode, results[i]);
            connection.cost = Vector3.Distance(fromNode, results[i]);
            connections[i] = connection;
        }
    }

    public void Draw()
    {

        if (tileSize.x == 0 || tileSize.y == 0 || tileSize.z == 0 || graphSize.x == 0 || graphSize.y == 0 || graphSize.z == 0) return;

        Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
        tileExtents = Vector3.Scale(tileSize, half);
        graphExtents = Vector3.Scale(graphSize, half);

        Vector3 min = new Vector3(transform.position.x - graphExtents.x, transform.position.y - graphExtents.y, transform.position.z - graphExtents.z);
        Vector3 max = new Vector3(transform.position.x + graphExtents.x, transform.position.y + graphExtents.y, transform.position.z + graphExtents.z);
       
        Vector3 tile = WorldToTileCenter(min + tileExtents);

       
        for (float x = 0; x < graphSize.x; x += tileSize.x)
        {
            for(float z = 0; z < graphSize.z; z+= tileSize.z)
            {

                Vector3 center = new Vector3(tile.x + x, tile.y, tile.z + z);
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
        
        /*
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
        */
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, graphSize);
    }
}
