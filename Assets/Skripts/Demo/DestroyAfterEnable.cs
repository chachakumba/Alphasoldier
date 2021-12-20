using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEnable : MonoBehaviour
{
    public float timeToDestroy = 1;
    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
