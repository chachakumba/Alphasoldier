using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomTrap : MonoBehaviour
{
    [SerializeField] List<Logic> enemies;
    [SerializeField] Door[] doors;
    bool activated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            foreach (Door door in doors)
            {
                door.CloseDoor();
            }
            foreach (Logic enemy in enemies)
            {
                if (enemy != null)
                    enemy.OnDeath += TrapClear;
            }
            activated = true;
        }
    }
    void TrapClear(object sender, System.EventArgs arg)
    {
        bool cleared = true;
        foreach (Logic enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.health > 0 && enemy.gameObject.activeInHierarchy)
                {
                    Debug.Log("Locked!");
                    cleared = false;
                    break;
                }
            }
        }
        if (cleared)
        {
            Debug.Log("OpenDoors!");
            foreach (Door door in doors)
            {
                door.OpenDoor();
            }
        }
    }
}
