using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallType
{
    Wall,
    Passage
}

public class Room : MonoBehaviour
{
    public Room[] neighbors;
    public WallType[] walls;

    public void SetWalls(WallType[] inWalls)
    {
        walls = new WallType[inWalls.Length];
        int i = 0;
        foreach(WallType w in inWalls)
        {
            walls[i++] = w;
        }
    }
}
