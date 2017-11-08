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

    public void Awake()
    {
        FindPath();
    }
    

    public void FindPath()
    {
        Init();

        while(numOpenNodes > 0)
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
            path = GetPath();
        }
        else
        {
            Debug.Log("No Path Found");
            path = null;
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

    public IntPoint GetSmallestOpenNode()
    {
        float smallestCost = tileWorld.nodeArray[priorityNodes[0].x, priorityNodes[0].y].estimatedTotalCost;
        IntPoint smallestNode = priorityNodes[0];
        for (int i = 1; i < numOpenNodes; i++)
        {
            if(tileWorld.nodeArray[priorityNodes[i].x, priorityNodes[i].y].estimatedTotalCost < smallestCost)
            {
                smallestCost = tileWorld.nodeArray[priorityNodes[i].x, priorityNodes[i].y].estimatedTotalCost;
                smallestNode = priorityNodes[i];
            }
        }

        return smallestNode;
    }


    public List<IConnection<IntPoint>> GetPath()
    {
        List<IConnection<IntPoint>> result = new List<IConnection<IntPoint>>();
        IntPoint pathNode = new IntPoint(currentNode.x,currentNode.y);
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
