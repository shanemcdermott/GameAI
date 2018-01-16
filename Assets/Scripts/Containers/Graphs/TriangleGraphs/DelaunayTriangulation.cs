using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DelaunayTriangle
{
    int A;
    int B;
    int C;
    private List<DelaunayTriangle> Children;

    public DelaunayTriangle(int A, int B, int C)
    {
        this.A = A;
        this.B = B;
        this.C = C;
        Children = new List<DelaunayTriangle>();
    }

    public void AddChild(DelaunayTriangle dt)
    {
        Children.Add(dt);
    }

    public void RemoveChild(DelaunayTriangle dt)
    {
        Children.Remove(dt);
    }

    public List<DelaunayTriangle> GetTriangles()
    {
        if (Children.Count == 0)
            return new List<DelaunayTriangle>(new DelaunayTriangle[] { new DelaunayTriangle(A, B, C) });
        return Children;
    }

    public IntPoint[] Edges()
    {
        return new IntPoint[] {new IntPoint(A,B), new IntPoint(B,C), new IntPoint(C,A) };
    }

    public bool SharesPointWith(DelaunayTriangle other)
    {
        return ContainsPoint(other.A) || ContainsPoint(other.B) || ContainsPoint(other.C);
    }

    public bool ContainsPoint(int a)
    {
        return a == A || a == B || a == C;
    }

    public bool ContainsEdge(IntPoint edge)
    {
        return ContainsEdge(edge.x, edge.y);
    }

    public bool ContainsEdge(int a, int b)
    {
        return (ContainsPoint(a) && ContainsPoint(b));
    }

    public override bool Equals(object obj)
    {
        if (!(obj is DelaunayTriangle))
            return false;

        DelaunayTriangle dt = (DelaunayTriangle)obj;
        // compare elements here
        return dt.ContainsPoint(A) && dt.ContainsPoint(B) && dt.ContainsPoint(C); 

    }
}

public class DelaunayTriangulation : MonoBehaviour
{
    protected List<Vector2> points;

    public DelaunayTriangle superTriangle;

    public void Init(Vector2 A, Vector2 B, Vector2 C)
    {
        points = new List<Vector2>(new Vector2[] {A,B,C});
        superTriangle = new DelaunayTriangle(0, 1, 2);
    }

    public void AddPoint(Vector2 NewPoint)
    {
        points.Add(NewPoint);
        List<DelaunayTriangle> badTriangles = new List<DelaunayTriangle>();
        // first find all the triangles that are no longer valid due to the insertion

        foreach (DelaunayTriangle dt in superTriangle.GetTriangles())
        {
            if (IsPointInsideTriangle(NewPoint, dt))
                badTriangles.Add(dt);
        }

        //Find the boundary of the polygonal hole
        List<IntPoint> polygon = new List<IntPoint>();
        foreach (DelaunayTriangle dt in badTriangles)
        {
            foreach (IntPoint edge in dt.Edges())
            {
                //if edge is not shared by any other triangles in badTriangles
                //add edge to polygon
                bool bIsShared = false;
                foreach(DelaunayTriangle bdt in badTriangles)
                {
                    if (bdt.Equals(dt)) continue;

                    bIsShared = bdt.ContainsEdge(edge);
                    if (bIsShared) break;
                }
                if(!bIsShared)
                    polygon.Add(edge);
            }
        }
        //Remove bad triangles from the triangulation
        foreach (DelaunayTriangle dt in badTriangles)
        {
            superTriangle.RemoveChild(dt);
        }

        int newPointIndex = points.Count - 1;
        //Re-triangulate the polygonal hole
        foreach(IntPoint edge in polygon)
        {
            DelaunayTriangle dt = new DelaunayTriangle(edge.x, edge.y, newPointIndex);
            superTriangle.AddChild(dt);
        }

        foreach(DelaunayTriangle dt in superTriangle.GetTriangles())
        {
            if (superTriangle.SharesPointWith(dt))
                superTriangle.RemoveChild(dt);
        }
        /**
         *      
      for each triangle in triangulation // done inserting points, now clean up
         if triangle contains a vertex from original super-triangle
            remove triangle from triangulation
        */
    }

    public bool IsPointInsideTriangle(Vector2 p, DelaunayTriangle dt)
    {
        //TODO
        return true;
    }

}
