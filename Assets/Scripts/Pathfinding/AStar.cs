using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<T> 
{


    public NodeRecord<T> current;
    public IGraph<T> graph;
    public T start;
    public T end;
    public Heuristic<T> heuristic;

    public PathfindingList<T> pathFindingList;


    public virtual List<BaseConnection<T>> PathFind(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        Init(graph, start, end, heuristic);


        while (pathFindingList.NumOpen() > 0)
        {
            //if ProcessNodes returns true, the goal was found.
            if (ProcessNodes(1))
            {
                return GetPath();
            }
        }

        //If we made it to here, no path was found.
        return null;
    }

    /// <summary>
    /// Initializes the pathfinding list and assigns all necessary variables.
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="heuristic"></param>
    public virtual void Init(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        this.graph = graph;
        this.start = start;
        this.end = end;
        this.heuristic = heuristic;
        
        //Initialize the open and closed lists
        pathFindingList = new PathfindingList<T>();
        pathFindingList.Build(graph, start, end, heuristic);
        this.current = pathFindingList.SmallestElement();
        //pathFindingList[start] = startRecord;

    }

    /// <summary>
    /// Process open nodes in search of the goal.
    /// </summary>
    /// <param name="numNodes">
    /// Number of open nodes to consider.
    /// </param>
    /// <return>
    /// Returns true if goal was found.
    /// </return>
    public virtual bool ProcessNodes(int numNodes)
    {
     
        for (int i = 0; i < numNodes && pathFindingList.HasOpenNodes(); i++)
        {
            //Get the smallest open node in the list.
            current = pathFindingList.SmallestElement();

            if (IsGoal(current)) return true;

            IConnection<T>[] links;
            graph.GetConnections(current.node, out links);

            foreach (IConnection<T> con in links)
            {
                T endNode = con.GetToNode();
                NodeRecord<T> endNodeRecord = pathFindingList.GetNodeRecord(endNode);

                float endNodeCost = current.costSoFar + con.GetCost();
                float endNodeHeuristic = 0f;
                

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

        return false;
    }


    public List<BaseConnection<T>> GetPath()
    {
        List<BaseConnection<T>> path = new List<BaseConnection<T>>();
        NodeRecord<T> record = new NodeRecord<T>();
        record.node = current.node;
        record.connection = current.connection;
        while (!record.node.Equals(start))
        {
            path.Add(record.connection);
            record = pathFindingList.GetNodeRecord(record.connection.GetFromNode());
        }

        path.Reverse();
        return path;
    }

    public bool IsGoal(NodeRecord<T> nodeRecord)
    {
        return nodeRecord.node.Equals(end);
    }
}
