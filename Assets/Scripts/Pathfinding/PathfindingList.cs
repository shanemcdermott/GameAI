using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList<T> : MonoBehaviour
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
        NodeRecord<T> startRecord = new NodeRecord<T>(
            start,
            null,
            0,
            heuristic.Estimate(start),
            NodeCategory.Open);

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
        int index = FindRecordIndex(record.node);
        if (index == -1)
        {
            Add(record);
        }
        else
        {
            if (record.category != contents[index].category)
            {
                if (contents[index].category == NodeCategory.Open)
                    openCount--;
                else if (record.category == NodeCategory.Open)
                    openCount++;
            }
            if (record.estimatedTotalCost < contents[smallestIndex].estimatedTotalCost)
                smallestIndex = index;

            contents[index] = record;
        }
    }

    public virtual void CloseRecord(T node)
    {
        int i = FindRecordIndex(node);
        if (i != -1)
        {
            NodeRecord<T> record = contents[i];
            if (record.category == NodeCategory.Open)
                openCount--;

            if (record.estimatedTotalCost < contents[smallestIndex].estimatedTotalCost)
                smallestIndex = i;

            record.category = NodeCategory.Closed;
            contents[i] = record;
            
        }
    }

    /// <summary>
    /// Find the smallest element in the list using the estimated total cost
    /// </summary>
    /// 
    /// <returns>
    /// The smallest element in the list.
    /// </returns>
    public NodeRecord<T> SmallestElement()
    {
        return contents[smallestIndex];
    }
    
    public int NumOpen()
    {
        return openCount;
    }

    public bool HasOpenNodes()
    {
        return openCount > 0;
    }

    private int FindRecordIndex(T node)
    {
        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i].node.Equals(node))
            {
                return i;
            }
        }
        return -1;
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
