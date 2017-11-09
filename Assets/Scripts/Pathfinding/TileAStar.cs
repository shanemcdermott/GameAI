using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAStar : MonoBehaviour
{
   
    public TileWorld tileWorld;
    public TileGraph tileGraph;
    public Transform startTransform;
    public Transform goalTransform;
    public Heuristic<IntPoint> heuristic;
    


    private List<IConnection<IntPoint>> path;

    private IntPoint startNode;
    private IntPoint goalNode;

    private IntPoint currentNode;

    private List<IntPoint> priorityNodes;
    public int numOpenNodes;

    public void Clear()
    {
        numOpenNodes = 0;
        tileWorld.ClearNodes();
        path = null;
        startNode = tileGraph.WorldToTile(startTransform.position);
        goalNode = tileGraph.WorldToTile(goalTransform.position);
        priorityNodes = new List<IntPoint>();
    }

    //Initialize tileGraph and clear out path
    public void Init()
    {
        numOpenNodes = 0;
        tileWorld.Init();
        path = null;
        startNode = tileGraph.WorldToTile(startTransform.position);
        goalNode = tileGraph.WorldToTile(goalTransform.position);
        priorityNodes = new List<IntPoint>();
        heuristic = new EuclideanHeuristic(goalNode);

        AddPriorityNode(startNode);


    }
    
    public bool FindAgentPath(GameObject agent, Transform goal)
    {
        startTransform = agent.transform;
        goalTransform = goal;
        return FindPath();
    }

    public bool FindPath()
    {
        Init();

        while(GetNumOpenNodes() > 0)
        {
            currentNode = GetSmallestOpenNode();
            TileRecord currentRecord = tileWorld.nodeArray[currentNode.x, currentNode.y];

            if (currentNode.Equals(goalNode))
                break;

            IConnection<IntPoint>[] neighbors;
            tileGraph.GetConnections(currentNode, out neighbors);
            foreach (IConnection<IntPoint> edge in neighbors)
            {
                IntPoint toNode = edge.GetToNode();
                TileRecord endRecord = tileWorld.nodeArray[toNode.x, toNode.y];
                float endNodeCost = currentRecord.costSoFar + edge.GetCost();
                float endNodeHeuristic = 0f;

                if (endRecord.category == NodeCategory.Closed)
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
                endRecord.edge = edge;
                endRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                endRecord.category = NodeCategory.Open;
                tileWorld.nodeArray[toNode.x,toNode.y] = endRecord;
                AddPriorityNode(toNode);
            }

            CloseNode(currentNode);
        }
        
      
        if(currentNode.Equals(goalNode))
        {
            Debug.Log("Found Path: ");
            path = GetPathConnections();
            return true;
        }
        else
        {
            Debug.Log("No Path Found");
            path = null;
            return false;
        }

        priorityNodes = null;
        numOpenNodes = 0;
    }
    
    public void AddPriorityNode(IntPoint node)
    {
        priorityNodes.Add(node);
        tileWorld.SetNodeCategory(node, NodeCategory.Open);
        numOpenNodes++;
    }

    public void CloseNode(IntPoint node)
    {
        priorityNodes.Remove(node);
        tileWorld.SetNodeCategory(node, NodeCategory.Closed);
        numOpenNodes--;
    }

    public int GetNumOpenNodes()
    {
        return priorityNodes.Count;
    }

    public IntPoint GetSmallestOpenNode()
    {
        
        IntPoint smallestNode = priorityNodes[0];
        float smallestCost = tileWorld.GetRecordAt(smallestNode).estimatedTotalCost;
        for (int i = 1; i < GetNumOpenNodes(); i++)
        {
            float cost = tileWorld.GetRecordAt(priorityNodes[i]).estimatedTotalCost;

            if (cost < smallestCost)
            {
                smallestCost = cost;
                smallestNode = priorityNodes[i];
            }
        }

        return smallestNode;
    }

    public List<IConnection<IntPoint>> GetPathConnections()
    {
        List<IConnection<IntPoint>> result = new List<IConnection<IntPoint>>();
        IntPoint pathNode = new IntPoint(currentNode.x, currentNode.y);
        IConnection<IntPoint> edge = tileWorld.nodeArray[pathNode.x, pathNode.y].edge;
        while (!pathNode.Equals(startNode))
        {
            result.Add(edge);
            pathNode = edge.GetFromNode();
            edge = tileWorld.nodeArray[pathNode.x, pathNode.y].edge;
        }

        result.Reverse();
        return result;
    }

    /// <summary>
    /// Returns list of path points in world coordinates.
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetPath()
    {
        List<Vector3> result = new List<Vector3>();
        IntPoint pathNode = new IntPoint(currentNode.x,currentNode.y);
        IConnection<IntPoint> edge = tileWorld.nodeArray[pathNode.x, pathNode.y].edge;

        result.Add(tileGraph.TileToWorld(pathNode));
        while (!pathNode.Equals(startNode))
        {
            pathNode = edge.GetFromNode();
            edge = tileWorld.nodeArray[pathNode.x, pathNode.y].edge;
            result.Add(tileGraph.TileToWorld(pathNode));
        }

        //result.Add(tileGraph.TileToWorld(edge.GetFromNode()));
        result.Reverse();
        return result;
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            foreach(IConnection<IntPoint> edge in path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(tileGraph.TileToWorld(edge.GetFromNode()), 0.5f * tileGraph.tileSize);
                
            }
        }
    }

}
