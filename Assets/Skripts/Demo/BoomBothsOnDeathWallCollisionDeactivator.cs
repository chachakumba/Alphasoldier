using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBothsOnDeathWallCollisionDeactivator : MonoBehaviour
{
    [SerializeField] GameObject spawner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DeathWall>() != null)
        {
            spawner.SetActive(false);
        }
    }
}
