using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<T> 
{

    public NodeRecord<T> startRecord;
    public NodeRecord<T> current;
    public IGraph<T> graph;
    public T start;
    public T end;
    public Heuristic<T> heuristic;

    public PathfindingList<T> pathFindingList;

    public virtual List<BaseConnection<T>> PathFind(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        Init(graph, start, end, heuristic);

        current = startRecord;
        while (Iterate(1))
        {}

        if (!current.node.Equals(end))
        {
            return null;
        }
        else
        {
            List<BaseConnection<T>> path = new List<BaseConnection<T>>();
            while (!current.node.Equals(start))
            {
                path.Add(current.connection);
                current = pathFindingList.GetNodeRecord(current.connection.GetFromNode());
            }

            path.Reverse();
            return path;
        }
    }

    public virtual void Init(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        this.graph = graph;
        this.start = start;
        this.end = end;
        this.heuristic = heuristic;
        //Initialize the record for the start node.
        startRecord = new NodeRecord<T>();
        startRecord.node = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;
        startRecord.estimatedTotalCost = heuristic.Estimate(start);
        startRecord.category = NodeCategory.Open;

        //Initialize the open and closed lists
        pathFindingList = new PathfindingList<T>();
        pathFindingList.Build(graph, start, end, heuristic);
        //pathFindingList[start] = startRecord;

    }

    public virtual bool Iterate(int numIterations)
    {
        int counter = 0;
        while (pathFindingList.NumOpen() > 0 && counter++ < numIterations)
        {
            current = pathFindingList.SmallestElement(NodeCategory.Open, heuristic);
            if (current.node.Equals(end)) break;

            IConnection<T>[] links;
            graph.GetConnections(current.node, out links);

            foreach (IConnection<T> con in links)
            {
                T endNode = con.GetToNode();
                float endNodeCost = current.costSoFar + con.GetCost();
                float endNodeHeuristic = 0f;
                NodeRecord<T> endNodeRecord = pathFindingList.GetNodeRecord(endNode);

                if (endNodeRecord.category == NodeCategory.Closed)
                {
                    if (endNodeRecord.costSoFar <= endNodeCost)
                        continue;

                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;

                }
                else if (endNodeRecord.category == NodeCategory.Open)
                {
                    if (endNodeRecord.costSoFar <= endNodeCost) continue;

                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }
                else
                {
                    endNodeHeuristic = heuristic.Estimate(endNode);
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = (BaseConnection<T>)con;
                endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                endNodeRecord.category = NodeCategory.Open;
                pathFindingList.UpdateRecord(endNodeRecord);
            }

            //We've finished looking at the connections for the current node,
            //so mark it as closed.
            current.category = NodeCategory.Closed;
            pathFindingList.UpdateRecord(current);
        }
        return pathFindingList.NumOpen() > 0;
    }
}
