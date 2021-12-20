using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlocker : MonoBehaviour
{
    [SerializeField] Door[] connectedDoors;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(Door door in connectedDoors)
        {
            door.CloseDoor();
        }
        enabled = false;
    }
}
