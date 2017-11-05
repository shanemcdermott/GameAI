using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclideanHeuristic : Heuristic<Vector3>
{

    /// <summary>
    /// Returns Euclidean distance between given node and goal node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override float Estimate(Vector3 node)
    {
        return Vector3.Distance(node, goal);
    }
}
