using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAwaker : MonoBehaviour
{
    [SerializeField]Boss[] bossSkripts;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Boss boss in bossSkripts)
        {
            boss.enabled = true;
        }
        enabled = false;
    }

}
