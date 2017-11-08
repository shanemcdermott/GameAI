using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWorld : MonoBehaviour
{
    public TileGraph tileGraph;
    public Vector3 graphSize = new Vector3(40,1,40);
    public bool drawRecords = false;
    public bool drawBlocking = false;

    public TileRecord[,] nodeArray;
    private IntPoint numTiles;
    private Vector3 tileDimensions;

    public void Init()
    {
        tileGraph.Init();
        numTiles = tileGraph.WorldToTile(graphSize);
        tileDimensions = new Vector3(tileGraph.tileSize, tileGraph.tileSize, tileGraph.tileSize);
        nodeArray = new TileRecord[numTiles.x, numTiles.y];
        for(IntPoint tile = new IntPoint(0,0); tile.x < numTiles.x; tile.x++)
        {
            for(tile.y=0; tile.y < numTiles.y; tile.y++)
            {
                nodeArray[tile.x, tile.y] = new TileRecord();
                nodeArray[tile.x, tile.y].category = NodeCategory.Unvisited;
            }
        }
    }

    public void SetNodeCategory(IntPoint node, NodeCategory category)
    {
        nodeArray[node.x, node.y].category = category;
    }


    public void OnDrawGizmos()
    {
        if(drawRecords)
        {
            for (IntPoint tile = new IntPoint(0, 0); tile.x < numTiles.x; tile.x++)
            {
                for (tile.y = 0; tile.y < numTiles.y; tile.y++)
                {
                    if(nodeArray[tile.x,tile.y].category == NodeCategory.Open)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(tileGraph.TileToWorld(tile), tileDimensions);
                    }
                    else if(nodeArray[tile.x,tile.y].category == NodeCategory.Closed)
                    {
                        Gizmos.color = Color.gray;
                        Gizmos.DrawCube(tileGraph.TileToWorld(tile), tileDimensions);
                    }

                    Gizmos.color = Color.black;
                    Gizmos.DrawWireCube(tileGraph.TileToWorld(tile), tileDimensions);

                }
            }

        }
        if(drawBlocking)
        {
            for (IntPoint tile = new IntPoint(0, 0); tile.x < numTiles.x; tile.x++)
            {
                for (tile.y = 0; tile.y < numTiles.y; tile.y++)
                {
                    if (tileGraph.IsTileBlocked(tile))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(tileGraph.TileToWorld(tile), tileDimensions);
                    }

                }
            }
        }
    }
}
