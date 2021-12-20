using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo3rdRoomManager : MonoBehaviour
{
    bool generated = false;
    [SerializeField] GameObject[] rooms;
    [SerializeField] int roomsAmount;
    [SerializeField] float roomLength = 20.75f;
    [SerializeField] Transform startPoint;
    [SerializeField] List<GameObject> spawnedRooms;
    public void GenerateRooms()
    {
        if (!generated)
        {
            int length = rooms.Length;
            for (int i = 0; i < roomsAmount; i++)
            {
                spawnedRooms.Add(Instantiate(rooms[Random.Range(0, length)], startPoint));
                spawnedRooms[i].transform.position += Vector3.right * roomLength * i;
            }
        }
    }
}
