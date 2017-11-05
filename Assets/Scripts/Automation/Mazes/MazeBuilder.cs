using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    //Builders
    public RoomBuilder roomBuilder;

    public GameObject rootObject;


    //Parameters
    public int numRoomsX = 4;
    public int numRoomsZ = 4;
    public Vector3 roomSize = new Vector3(10, 0, 10);

    public Vector2 start = new Vector2();
    public Vector2 end = new Vector2();

    //Results
    public GameObject[] rooms = new GameObject[16];


    public void ClearMaze()
    {
        for (int x = 0; x < rooms.Length; x++)
        {
            if (rooms[x] != null)
                DestroyImmediate(rooms[x]);
            
        }
    }

    public Vector3 GetOffset()
    {
        return rootObject.transform.position + new Vector3((1 + numRoomsX) * -0.5f, 0, (1 + numRoomsZ) * -0.5f);
    }


    public virtual void BuildMaze()
    {
        ClearMaze();

        rootObject.transform.position = transform.position;

        rooms = new GameObject[numRoomsX*numRoomsZ];
        Vector3 offset = GetOffset();
        int i = 0;
        for (int x = 0; x < numRoomsX; x++)
        {
            //roomBuilder.walls[3] = roomBuilder.walls[2] = WallType.Wall;

            for (int z = 0; z < numRoomsZ; z++)
            {
                Vector3 spawnPoint = new Vector3(x, 1, z);
                spawnPoint.Scale(roomSize);
                spawnPoint += offset;

                if(x>0)
                    roomBuilder.walls[3] = roomBuilder.walls[1];
                if(z>0)
                    roomBuilder.walls[2] = roomBuilder.walls[0];

                ChoosePassage();

                roomBuilder.BuildRoom(rootObject.transform);
                rooms[i] = roomBuilder.rootObject;
                rooms[i].transform.position = spawnPoint;
                rooms[i].transform.SetParent(rootObject.transform);
                i++;
            }
            //rooms[i] = Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);
            //rooms[i].transform.SetParent(transform);          
        }


    }

    public virtual void ChoosePassage()
    {
      if (Random.value > 0.5)
        {
            roomBuilder.walls[0] = WallType.Passage;
            roomBuilder.walls[1] = WallType.Wall;
        }
        else
        {
            roomBuilder.walls[0] = WallType.Wall;
            roomBuilder.walls[1] = WallType.Passage;
        }
     
    }

    public virtual void CarvePassages()
    {

    }

}
