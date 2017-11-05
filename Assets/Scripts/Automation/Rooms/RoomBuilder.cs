using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomBuilder : MonoBehaviour
{
    //Prefabs
    public GameObject floor;
    public GameObject wall;
    public GameObject corner;

    //params
    public GameObject rootObject;

    public int numX = 10;
    public int numZ = 10;


    public float floorOffset = 0.5f;
    public WallType[] walls = new WallType[4];


    //Results
    public List<GameObject> builtWalls = new List<GameObject>();
    public GameObject[] builtCorners = new GameObject[4];

    public void ClearWalls()
    {
        foreach(GameObject w in builtWalls)
        {
            if (w == null) break;
            DestroyImmediate(w);
        }

        builtWalls.Clear();
    }

    public void ClearCorners()
    {
        foreach (GameObject c in builtCorners)
        {
            if (c != null)
                DestroyImmediate(c);

        }
    }

    public void BuildRoom(Transform t)
    {
        rootObject = Instantiate(floor, t.position, Quaternion.identity);
        BuildWalls();
        Room r = rootObject.GetComponent<Room>();
        r.SetWalls(walls);
    }

    public void BuildRoom()
    {
        BuildRoom(transform);
    }

    public void BuildWalls()
    {
        BuildWallsOnX();
        BuildWallsOnZ();
        AddCorners();
    }

    public Vector3 GetOffset()
    {
        return rootObject.transform.position + new Vector3((1 + numX) * -0.5f, floorOffset, (1 + numZ) * -0.5f);
    }

    private void BuildWallsOnX()
    {
        Vector3 offset = GetOffset();
        offset.x++;
        Quaternion qSouth = Quaternion.identity;
        Quaternion qNorth = Quaternion.identity;
        //Quaternion qNorth = Quaternion.Euler(180, 0, 0);

        for (int x = 0; x < numX; x++)
        {
            
            Vector3 spawnPoint = new Vector3(offset.x + x, offset.y, offset.z);
            if (walls[2] == WallType.Wall || x != numX / 2)
                AttachWall(Instantiate(wall, spawnPoint, qNorth));

            if (walls[0]==WallType.Passage && x == numX / 2) continue;
            spawnPoint.z += (numZ + 1);
            AttachWall(Instantiate(wall, spawnPoint, qSouth));
        }
    }

    private void BuildWallsOnZ()
    {
        Vector3 offset = GetOffset();
        offset.z++;
        Quaternion qEast = Quaternion.Euler(0, -90, 0);
        Quaternion qWest = Quaternion.Euler(0, 90, 0);
        for (int z = 0; z < numZ; z++)
        {
            Vector3 spawnPoint = new Vector3(offset.x, offset.y, offset.z + z);

            if(walls[3]==WallType.Wall || z != numZ / 2)
                AttachWall(Instantiate(wall, spawnPoint, qEast));

            if (walls[1]==WallType.Passage && z == numZ / 2) continue;

            spawnPoint.x += (numX + 1);

            AttachWall(Instantiate(wall, spawnPoint, qWest));

        }
    }

    private void AttachWall(GameObject inWall)
    {
        inWall.transform.SetParent(rootObject.transform);
        builtWalls.Add(inWall);
    }

    private void AddCorners()
    {
        
        Vector3 spawnLoc = GetOffset();
        builtCorners[0] = Instantiate(corner, spawnLoc, Quaternion.identity);
        spawnLoc.x += numX + 1;
        builtCorners[1] = Instantiate(corner, spawnLoc, Quaternion.identity);
        spawnLoc.z += numZ + 1;
        builtCorners[2] = Instantiate(corner, spawnLoc, Quaternion.identity);
        spawnLoc.x -= (1 + numX);
        builtCorners[3] = Instantiate(corner, spawnLoc, Quaternion.identity);

        foreach(GameObject c in builtCorners)
        {
            c.transform.SetParent(rootObject.transform);
        }
    }
}
