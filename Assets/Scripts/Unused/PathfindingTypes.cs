using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPathfindingList<T>
{
    void AddRecord(NodeRecord<T> record);
    void UpdateRecord(NodeRecord<T> record);

    void CloseRecord(T node);
    NodeRecord<T> SmallestElement();

    int NumOpenRecords();

    NodeRecord<T> FindNodeRecord(T node);

    bool ContainsNodeRecord(T node);

}



public struct NodeRecord<T>
{
    public T node;
    public IConnection<T> connection;
    public float costSoFar;
    public float estimatedTotalCost;
    public NodeCategory category;
    
    public NodeRecord(T node) : this()
    {
        this.node = node;
        this.costSoFar = 0;
        this.estimatedTotalCost = 0;
        this.category = NodeCategory.Unvisited;
    }

    public NodeRecord(T node, IConnection<T> connection, float costSoFar, float estTotalCost, NodeCategory category)
    {
        this.node = node;
        this.connection = connection;
        this.costSoFar = costSoFar;
        this.estimatedTotalCost = estTotalCost;
        this.category = category;
    }

}
