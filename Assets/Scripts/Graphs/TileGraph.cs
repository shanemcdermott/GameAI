using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraph : MonoBehaviour, IGraph<IntPoint>
{
    //Size of each tile
    public float tileSize = 1.0f;
    public string[] blocking = new string[1];

    private Vector3 tileExtents; 

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        tileExtents = new Vector3(tileSize * 0.5f, tileSize * 0.5f, tileSize * 0.5f);
        blocking[0] = "Blocking";
    }

    public IntPoint WorldToTile(Vector2 worldPosition)
    {
        return new IntPoint(worldPosition.x / tileSize, worldPosition.y / tileSize);
    }

    public IntPoint WorldToTile(Vector3 worldPosition)
    {
        return new IntPoint(worldPosition.x / tileSize, worldPosition.z / tileSize);
    }

    public Vector3 TileToWorld(IntPoint tilePosition)
    {
        return new Vector3(tilePosition.x * tileSize, transform.position.y, tilePosition.y * tileSize);
    }

    public Vector3 WorldToTileCenter(Vector3 worldPosition)
    {
        return TileToWorld(WorldToTile(worldPosition));
    }

    /// <summary>
    /// Fills connections with node connections that can be reached from fromNode
    /// </summary>
    /// <param name="fromNode">The tile coordinates of the node to find connections for.</param>
    /// <param name="connections">The resulting set of connections</param>
    public void GetConnections(IntPoint fromNode, out IConnection<IntPoint>[] connections)
    {
        List<IntPoint> results = new List<IntPoint>();
        
        for (int x = -1; x <= 1; x += 1)
        {
            for (int y = -1; y <= 1; y += 1)
            {
                if (x == 0 && y == 0) continue;

                IntPoint tile = IntPoint.Add(fromNode, x, y);
              
                if(IsTileOpen(tile))
                    results.Add(tile);
            }

        }
        
        connections = new IConnection<IntPoint>[results.Count];
        for (int i = 0; i < results.Count; i++)
        {
            BaseConnection<IntPoint> connection = new BaseConnection<IntPoint>(fromNode, results[i]);
            connections[i] = connection;
        }
    }

    public bool IsTileOpen(Vector3 worldPosition)
    {
        Vector3 adjustedPosition = WorldToTileCenter(worldPosition);
       
        Collider[] hits = Physics.OverlapBox(adjustedPosition, tileExtents, Quaternion.identity, LayerMask.GetMask(blocking));
        return hits.Length == 0;
    }

    public bool IsTileOpen(IntPoint tileCoords)
    {
        Vector3 worldNode = TileToWorld(tileCoords);
        Collider[] hits = Physics.OverlapBox(worldNode, tileExtents, Quaternion.identity, LayerMask.GetMask(blocking));
        return hits.Length == 0;
    }

}
