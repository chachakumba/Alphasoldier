using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPartsDisabler : MonoBehaviour
{
    public static LevelPartsDisabler instance;
    public Transform[] playerLoadPos;
    public GameObject[] levelParts;
    public LevelPartCollider[] colliders;
    private void Awake()
    {
        instance = this;
        colliders = GetComponentsInChildren<LevelPartCollider>();
        foreach(LevelPartCollider collider in colliders)
        {
            collider.levelDis = this;
        }
    }
    public void CheckParts()
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].isPartActive)
            {
                ActivatePart(i);
            }
            else
            {
                DeactivatePart(i);
            }
        }
    }
    public void ActivatePart(int index)
    {
        levelParts[index].SetActive(true);
    }
    public void DeactivatePart(int index)
    {
        levelParts[index].SetActive(false);
    }
}
