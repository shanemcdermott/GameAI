using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Algorithm from http://devmag.org.za/2009/05/03/poisson-disk-sampling/
// Shane McDermott 2018

public class PoissonDiskSampling : PointGenerator
{

    public float width = 100;
    public float height = 100;
    //Minimum distance between points
    public float minDistance = 5;
    //How many new points each new point should try to spawn
    public int newPointsCount = 30;
    //Target total number of points to generate
    public int maxTotalPoints = 1000;

    [SerializeField]
    private float cellSize;
    private Rect bounds;
    public Grid2D grid;

    private RandomQueue<Vector2> processList;

    public List<Vector2> samplePoints;

    public void Init()
    {
        cellSize = minDistance / Mathf.Sqrt(2);
        grid = new Grid2D(Mathf.CeilToInt(width / cellSize), Mathf.CeilToInt(height / cellSize));
        processList = new RandomQueue<Vector2>();
        samplePoints = new List<Vector2>();
        bounds = new Rect(0, 0, width, height);
    }

    public Vector2 MakeRandomPoint()
    {
        float x = UnityEngine.Random.Range(0, width);
        float y = UnityEngine.Random.Range(0, height);
        return new Vector2(x, y);
    }

    public void GeneratePoisson()
    {
        Init();

        AddPoint(MakeRandomPoint());
        while (!processList.Empty())
        {
            Vector2 point = processList.Pop();
            for (int i = 0; i < newPointsCount && samplePoints.Count < maxTotalPoints; i++)
            {
                Vector2 newPoint = GenerateRandomPointAround(point, minDistance);
               if(bounds.Contains(newPoint) && !IsInNeighborhood(newPoint))
                {
                    AddPoint(newPoint);
                }
            }
        }
        Debug.Log("Generated " + samplePoints.Count + " points with Poisson Disk Sampling.");
    }

    private bool IsInNeighborhood(Vector2 newPoint)
    {

        IntPoint gridPoint = PointToGridCoord(newPoint);
        Vector2[,] square;
        int squareSize = 5;
        grid.SquareAroundPoint(gridPoint, squareSize, out square);
        for (int x = 0; x < squareSize; x++)
        {
            for (int y = 0; y < squareSize; y++)
            {
                if (square[x, y] != Vector2.positiveInfinity && Vector2.Distance(square[x, y], newPoint) < minDistance)
                    return true;
            }
        }

        return false;
    }

    public void AddPoint(Vector2 InPoint)
    {
        processList.Push(InPoint);
        samplePoints.Add(InPoint);
        grid.SetPoint(PointToGridCoord(InPoint), InPoint);
    }

    public Vector2 GenerateRandomPointAround(Vector2 point, float minDist)
    {
        float r1 = UnityEngine.Random.value;
        float r2 = UnityEngine.Random.value;

        float radius = minDist * (r1 + 1);
        float angle = 2 * Mathf.PI * r2;

        float newX = point.x + radius * Mathf.Cos(angle);
        float newY = point.y + radius * Mathf.Sin(angle);

        return new Vector2(newX, newY);
    }

    public IntPoint PointToGridCoord(Vector2 point)
    {
        float gridX = (int)(point.x / cellSize);
        float gridY = (int)(point.y / cellSize);
        return new IntPoint(gridX, gridY);
    }

    public override void Generate(out List<Vector2> results)
    {
        GeneratePoisson();
        results = samplePoints;
    }

    public override void Generate(float width, float height, float minDstance, int maxPointsDesired, out List<Vector2> results)
    {
        this.width = width;
        this.height = height;
        this.minDistance = minDstance;
        this.maxTotalPoints = maxPointsDesired;
        GeneratePoisson();
        results = samplePoints;
    }
}
