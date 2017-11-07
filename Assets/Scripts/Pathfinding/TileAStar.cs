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
        if(astar == null)
        {
            astar = new AStar<Vector3>();
            astar.Init(tileGraph, start.position, goal.position, new EuclideanHeuristic(this.goal.position));
        }
        
        path = astar.PathFind(tileGraph, start.position, goal.position, new EuclideanHeuristic(this.goal.position));
        if(path != null)
        {
            Debug.Log("Found Path: " + path);

        }
    }

    public void Iterate()
    {
        if (astar == null)
        {
            astar = new AStar<Vector3>();
        }
        if(astar.pathFindingList == null)
        {
            astar.Init(tileGraph, start.position, goal.position, new EuclideanHeuristic(this.goal.position));
        }

        astar.Iterate(numIterations);

        path = new List<BaseConnection<Vector3>>();
        NodeRecord<Vector3> record = astar.current;
        while (!record.node.Equals(start) && record.node != null)
        {
            path.Add(record.connection);
            record = astar.pathFindingList.GetNodeRecord(record.connection.GetFromNode());
        }

        path.Reverse();

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
            Gizmos.DrawSphere(path[0].fromNode, tileGraph.tileSize.x);
            foreach(BaseConnection<Vector3> con in path)
            {

                Gizmos.color = Color.black;
                Gizmos.DrawLine(con.fromNode, con.toNode);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(con.toNode, tileGraph.tileSize.x);
            }
        }
    }

}
