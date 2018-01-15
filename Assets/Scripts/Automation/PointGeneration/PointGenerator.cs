using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PointGenerator : MonoBehaviour
{

   public abstract void Generate(out List<Vector2> results);
   public abstract void Generate(float width, float height, float minDstance, int maxPointsDesired, out List<Vector2> results);
}