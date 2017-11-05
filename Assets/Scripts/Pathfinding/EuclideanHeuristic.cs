using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuclideanHeuristic : Heuristic
{

    /// <summary>
    /// Returns Euclidean distance between given node and goal node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override float Estimate(int node)
    {
        GameObject A = World.GetObject(node);
        GameObject B = World.GetObject(goalNode);

        return Vector3.Distance(A.transform.position, B.transform.position);
    }
}
