using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TriggerActivateObjectst : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDectivate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject obj in objectsToActivate) obj.SetActive(true);
        foreach (GameObject obj in objectsToDectivate) obj.SetActive(false);
    }
}
