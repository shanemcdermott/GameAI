using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList<T>
{
    private List<NodeRecord<T>> contents;
    private int openCount;
    private int smallestIndex;
    private T start;
    private T goal;

    public virtual void Build(IGraph<T> graph, T start, T end, Heuristic<T> heuristic)
    {
        contents = new List<NodeRecord<T>>();
        openCount = 1;
        smallestIndex = 0;
        this.start = start;
        this.goal = end;
        NodeRecord<T> startRecord = new NodeRecord<T>(start, null, 0, heuristic.Estimate(end), NodeCategory.Open);

        contents.Add(startRecord);
       
    }

    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add.</param>
    public virtual void Add(NodeRecord<T> record)
    {
        contents.Add(record);
        if (record.category == NodeCategory.Open)
            openCount++;
        if (record.estimatedTotalCost < contents[smallestIndex].estimatedTotalCost)
            smallestIndex = contents.Count - 1;
    }

    public virtual void UpdateRecord(NodeRecord<T> record)
    {
        bool found = false;
        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i].node.Equals(record.node))
            {
                if (record.category != contents[i].category)
                {
                    if (contents[i].category == NodeCategory.Open)
                        openCount--;
                    else if (record.category == NodeCategory.Open)
                        openCount++;
                }
                if (record.estimatedTotalCost < contents[smallestIndex].estimatedTotalCost)
                    smallestIndex = i;

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
    /// Find the smallest element in the list using the estimated total cost
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public NodeRecord<T> SmallestElement(NodeCategory category, Heuristic<T> heuristic)
    {
        return contents[smallestIndex];
    }
    
    public int NumOpen()
    {
        return openCount;
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
