using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAStar : MonoBehaviour
{
    public TileGraph tileGraph;
    public Transform startTransform;
    public Transform goalTransform;
    public Heuristic<IntPoint> heuristic;
    
    public int numIterations = 1;

    public bool drawGraph = false;
    public bool drawPath = false;

    public NodeRecord<IntPoint> currentRecord;

    private IPathfindingList<IntPoint> nodes;

    private List<BaseConnection<IntPoint>> path;

    private IntPoint startNode;
    private IntPoint goalNode;

    //Initialize tileGraph and clear out path
    public void Init()
    {
        tileGraph.Init();
        path = null;
        startNode = tileGraph.WorldToTile(startTransform.position);
        goalNode = tileGraph.WorldToTile(goalTransform.position);
        nodes = new PathfindingList<IntPoint>();

        NodeRecord<IntPoint> startRecord = new NodeRecord<IntPoint>(
            startNode,
            null,
            0,
            heuristic.Estimate(startNode),
            NodeCategory.Open);

        nodes.AddRecord(startRecord);
    }

    public void FindPath()
    {
        Init();

        while(nodes.NumOpenRecords() > 0)
        {
            currentRecord = nodes.SmallestElement();
            //We're finished!
            if (currentRecord.node.Equals(goalNode))
                break;

            IConnection<IntPoint>[] neighbors;
            tileGraph.GetConnections(currentRecord.node, out neighbors);
            
            foreach(IConnection<IntPoint> edge in neighbors)
            {
                IntPoint toNode = edge.GetToNode();
                NodeRecord<IntPoint> endRecord = nodes.FindNodeRecord(toNode);
                float endNodeCost = currentRecord.costSoFar + edge.GetCost();
                float endNodeHeuristic = 0f;

                if(endRecord.category == NodeCategory.Closed)
                {
                    if (endRecord.costSoFar <= endNodeCost)
                        continue;
                    endNodeHeuristic = endNodeHeuristic = endRecord.estimatedTotalCost - endRecord.costSoFar;
                }
                else if (endRecord.category == NodeCategory.Open)
                {
                    if (endRecord.costSoFar <= endNodeCost) continue;

                    endNodeHeuristic = endRecord.estimatedTotalCost - endRecord.costSoFar;
                }
                else
                {
                    endNodeHeuristic = heuristic.Estimate(toNode);
                }

                endRecord.costSoFar = endNodeCost;
                endRecord.connection = edge;
                endRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                endRecord.category = NodeCategory.Open;
                nodes.UpdateRecord(endRecord);
            }

            currentRecord.category = NodeCategory.Closed;
            nodes.UpdateRecord(currentRecord);

        }
      

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
