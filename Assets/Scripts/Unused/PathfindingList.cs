using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingList<T> : MonoBehaviour, IPathfindingList<T>
{
    private List<NodeRecord<T>> contents = new List<NodeRecord<T>>();
    private int openCount;
    private int smallestIndex;



    /// <summary>
    /// Adds the record to the list.
    /// </summary>
    /// <param name="record">The record to add.</param>
    public void AddRecord(NodeRecord<T> record)
    {
        contents.Add(record);
        if (record.category == NodeCategory.Open)
            openCount++;
        if (record.estimatedTotalCost < contents[smallestIndex].estimatedTotalCost)
            smallestIndex = contents.Count - 1;
    }

    //BUGGY
    public void UpdateRecord(NodeRecord<T> record)
    {
        int index = FindRecordIndex(record.node);
        if (index == -1)
        {
            AddRecord(record);
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

    public void CloseRecord(T node)
    {
        int i = FindRecordIndex(node);
        if (i != -1)
        {
            NodeRecord<T> record = contents[i];
            if (record.category == NodeCategory.Open)
                openCount--;

            /*
            if (i == smallestIndex)
                FindSmallest();
                */
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
    
    public int NumOpenRecords()
    {
        return openCount;
    }

    public bool HasOpenNodes()
    {
        return openCount > 0;
    }

    public bool ContainsNodeRecord(T node)
    {
        int i = FindRecordIndex(node);
        return i != -1;
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

    public NodeRecord<T> FindNodeRecord(T node)
    {
        int i = FindRecordIndex(node);
        if (i == -1)
            return new NodeRecord<T>(node);
        else
            return contents[i];
    }

}
