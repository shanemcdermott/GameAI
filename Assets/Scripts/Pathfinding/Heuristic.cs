using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic : MonoBehaviour
{

    public int goalNode;

    public virtual float Estimate(int node)
    {
        return 1f;
    }

}
