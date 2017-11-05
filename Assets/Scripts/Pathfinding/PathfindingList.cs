using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList
{
    private List<NodeRecord> contents;


    public virtual void Build(BaseGraph graph, int start, int end, Heuristic heuristic)
    {
        contents = new List<NodeRecord>(graph.nodes.Length);

        for(int i = 0; i < graph.nodes.Length; i++)
        {
            NodeRecord record = new NodeRecord();
            record.node = i;
            record.connection = null;
            record.costSoFar = 0;
            //record.estimatedTotalCost = heuristic.Estimate(i);
            //record.category = NodeCategory.Open;
            contents[i] = record;
        }
        
        //contents[start].estimatedTotalCost = heuristic.Estimate(start);
    }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add.</param>
    public virtual void Add(NodeRecord record)
    {
        contents.Add(record);
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
    public virtual bool Remove(NodeRecord record)
    {
        return contents.Remove(record);
    }

    /// <summary>
    /// Find the smallest element in the list using the estimated total cost
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public NodeRecord SmallestElement(NodeCategory category, Heuristic heuristic)
    {
        return contents[0];
    }
    
    /// <summary>
    /// Indexer definition to allow [] access.
    /// </summary>
    /// <param name="index">
    /// The index of the element to access.
    /// </param>
    /// <returns></returns>
    public NodeRecord this[int index]
    {
        get
        {
            return (contents[index]);
        }
        set
        {
            if (0 <= index && index < contents.Count)
            {
                contents[index] = value;
            }
            else
            {
                Debug.Log("<color=red>Error:</color> NodeRecord index out of bounds: " + index);
            }
        }
    }
  
}
