using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraph<T>
{
    /// <summary>
    /// Returns an array of connections (implementing IConnection)
    /// outgoing from the given node.
    /// </summary>
    /// <param name="fromNode">The node whose connections to return.</param>
    /// <param name="connections">The connections to return.</param>
    void GetConnections(T fromNode, out IConnection<T>[] connections);
}

public interface IHasConnections<K,V> where V : IConnection<K>
{
    /// <summary>
    /// Returns an array of outgoing connections (implementing IConnection)
    /// </summary>
    /// <param name="connections"></param>
    void GetConnections(out V[] connections);
}

//Connection between nodes of type T
public interface IConnection<T>
{
    /// <summary>
    /// Returns the non-negative cost of the connection
    /// </summary>
    /// <returns></returns>
    float GetCost();

    /// <summary>
    /// Returns the id of the node that this connection from.
    /// </summary>
    /// <returns></returns>
    T GetFromNode();

    /// <summary>
    /// Returns the id of the node that this connection leads to.
    /// </summary>
    /// <returns></returns>
    T GetToNode();
}

public enum NodeCategory
{
    Unvisited,
    Open,
    Closed
}

public struct NodeRecord
{
    public int node;
    public BaseConnection<int> connection;
    public float costSoFar;
    public float estimatedTotalCost;
    public NodeCategory category;
    
    public NodeRecord(int node, BaseConnection<int> connection, float costSoFar, float estTotalCost, NodeCategory category)
    {
        this.node = node;
        this.connection = connection;
        this.costSoFar = costSoFar;
        this.estimatedTotalCost = estTotalCost;
        this.category = category;
    }

}