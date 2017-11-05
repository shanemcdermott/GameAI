using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList<T>
{
    private List<NodeRecord<T>> contents;


    public virtual void Build(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        contents = new List<NodeRecord<T>>();

        /*
        for(int i = 0; i < graph.nodes.Length; i++)
        {
            NodeRecord<int> record = new NodeRecord<int>();
            record.node = i;
            record.connection = null;
            record.costSoFar = 0;
            //record.estimatedTotalCost = heuristic.Estimate(i);
            //record.category = NodeCategory.Open;
            contents[i] = record;
        }
        */
        
        //contents[start].estimatedTotalCost = heuristic.Estimate(start);
    }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add.</param>
    public virtual void Add(NodeRecord<T> record)
    {
        contents.Add(record);
    }

    public virtual void Update(NodeRecord<T> record)
    {
        bool found = false;
        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i].node.Equals(record.node))
            {
                contents[i] = record;
                found = true;
                break;
            }
        }
        if(!found)
        {
            Add(record);
        }
    }

    /// <summary>
    /// Removes the record from the list and returns true if successful.
    /// </summary>
    /// <param name="record">
    /// The record to remove.
    /// </param>
    /// <returns>
    /// true if successful.
    /// </returns>
    public virtual bool Remove(NodeRecord<T> record)
    {
        return contents.Remove(record);
    }

    /// <summary>
    /// Find the smallest element in the list using the estimated total cost
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public NodeRecord<T> SmallestElement(NodeCategory category, Heuristic<T> heuristic)
    {
        NodeRecord<T> smallest = contents[0];

        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i].category != category)
                continue;

            if (contents[i].estimatedTotalCost < smallest.estimatedTotalCost)
                smallest = contents[i];
               
        }

        return smallest;
    }
    
    public NodeRecord<T> GetNodeRecord(T node)
    {
        for(int i = 0; i < contents.Count; i++)
        {
            if (contents[i].node.Equals(node))
                return contents[i];
        }
        return new NodeRecord<T>();
    }
  
}
