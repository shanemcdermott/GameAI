using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public NodeRecord startRecord;
    private PathfindingList pathFindingList;

    public virtual List<BaseConnection<int>> PathFind(BaseGraph graph, int start, int end, Heuristic heuristic)
    {
        //Initialize the record for the start node.
        startRecord = new NodeRecord();
        startRecord.node = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;
        startRecord.estimatedTotalCost = heuristic.Estimate(start);
        startRecord.category = NodeCategory.Open;

        //Initialize the open and closed lists
        pathFindingList = new PathfindingList();
        pathFindingList.Build(graph, start, end, heuristic);
        pathFindingList[start] = startRecord;

        NodeRecord current = startRecord;

        while (true)//lengthopen>0)
        {
            current = pathFindingList.SmallestElement(NodeCategory.Open, heuristic);
            if (current.node == end) break;

            IConnection<int>[] links;
            graph.GetConnections(current.node, out links);

            foreach(IConnection<int> con in links)
            {
                int endNode = con.GetToNode();
                float endNodeCost = current.costSoFar + con.GetCost();
                float endNodeHeuristic = 0f;
                NodeRecord endNodeRecord = pathFindingList[endNode];


                if (pathFindingList[endNode].category==NodeCategory.Closed)
                {
                    if (pathFindingList[endNode].costSoFar <= endNodeCost)
                        continue;
    
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                    
                }
                else if(pathFindingList[endNode].category==NodeCategory.Open)
                {
                    if (pathFindingList[endNode].costSoFar <= endNodeCost) continue;

                    endNodeHeuristic = pathFindingList[endNode].estimatedTotalCost - pathFindingList[endNode].costSoFar;
                }
                else
                {
                    endNodeHeuristic = heuristic.Estimate(endNode);
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = (BaseConnection<int>)con;
                endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                endNodeRecord.category = NodeCategory.Open;
                pathFindingList[endNode] = endNodeRecord;



            }

            //We've finished looking at the connections for the current node,
            //so mark it as closed.
            current.category = NodeCategory.Closed;
            pathFindingList[current.node] = current;

        }

        if(current.node != end)
        {
            return null;
        }
        else
        {
            List<BaseConnection<int>> path = new List<BaseConnection<int>>();
            while(current.node != start)
            {
                path.Add(current.connection);
                current = pathFindingList[current.connection.GetFromNode()];
            }

            path.Reverse();
            return path;
        }
    }

}
