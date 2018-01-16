using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PointGenerator : MonoBehaviour
{
    [SerializeField]
    protected Rect bounds = new Rect(0,0,100,100);

    public void SetBounds(float width, float height)
    {
        bounds = new Rect(0, 0, width, height);
    }

    public virtual Vector2 NextPoint()
    {
        float x = UnityEngine.Random.Range(0, bounds.width);
        float y = UnityEngine.Random.Range(0, bounds.height);
        return new Vector2(x, y);
    }
   public abstract void Generate(out List<Vector2> results);
   public abstract void Generate(float width, float height, float minDstance, int maxPointsDesired, out List<Vector2> results);
}