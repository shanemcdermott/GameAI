using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAStar : MonoBehaviour
{
    public TileGraph tileGraph;
    public Transform start;
    public Transform goal;
    public int numIterations = 1;

    public bool drawGraph = true;
    public bool drawPath = false;

    private AStar<Vector3> astar = new AStar<Vector3>();
    private List<BaseConnection<Vector3>> path;

    public void FindPath()
    {
        astar = new AStar<Vector3>();
        Vector3 startPos = tileGraph.WorldToTileCenter(start.position);
        Vector3 goalPos = tileGraph.WorldToTileCenter(goal.position);     
        path = astar.PathFind(tileGraph, startPos, goalPos, new EuclideanHeuristic(goalPos));

        if (path != null)
        {
            Debug.Log("Found Path: " + path);
        }
        else
        {
            Debug.Log("No Path Found");
        }
    }

    public void Restart()
    {
        astar = null;
    }

    public void Iterate()
    {
        if (astar == null)
        {
            astar = new AStar<Vector3>();
        }
        if(astar.pathFindingList == null)
        {
            Vector3 startPos = tileGraph.WorldToTileCenter(start.position);
            Vector3 goalPos = tileGraph.WorldToTileCenter(goal.position);
            astar.Init(tileGraph, startPos, goalPos, new EuclideanHeuristic(goalPos));
        }

        if(astar.ProcessNodes(numIterations))
        {
            Debug.Log("Found Path!");
            
        }

        path = astar.GetPath();
        if(path != null)
        {
            Debug.Log("Current Path: " + path);
        }

    }

    public void ShowConnections()
    {
        BoxCollider[] children = tileGraph.GetComponents<BoxCollider>();
        foreach(BoxCollider box in children)
        {
            DestroyImmediate(box);
        }
       // tileGraph.CreateGraph();
    }

    public void OnDrawGizmos()
    {
        if(drawGraph)
        {
            tileGraph.Draw();
        }
        if(drawPath)
        {
            Gizmos.color = Color.blue;
            if(path[0].fromNode != null)
                Gizmos.DrawSphere(path[0].fromNode, tileGraph.tileExtents.x);

            foreach (BaseConnection<Vector3> con in path)
            {

                Gizmos.color = Color.black;
                Gizmos.DrawLine(con.fromNode, con.toNode);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(con.toNode, tileGraph.tileExtents.x);
            }
        }
    }

}
