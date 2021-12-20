using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourShooter : MonoBehaviour
{
    [SerializeField] GameObject spawnedPrefab;
    List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField] float spawnTime = 8;
    float time = 0;
    [SerializeField] float startRot = 0;
    private void Update()
    {
        if (time < Time.time)
        {
            time = Time.time + spawnTime;
            Shoot();
        }
    }
    void Shoot()
    {
        spawnedObjects.Add(Instantiate(spawnedPrefab, transform.position + Vector3.up * Random.Range(-transform.lossyScale.y/2, transform.lossyScale.y/2), Quaternion.identity));
        spawnedObjects[spawnedObjects.Count - 1].GetComponent<TrapBullet>().OnDestroy += RemoveFromArray;
        spawnedObjects[spawnedObjects.Count - 1].transform.eulerAngles = Vector3.forward * startRot;
    }
    void RemoveFromArray(object obj, System.EventArgs e)
    {
        spawnedObjects.Remove(obj as GameObject);
    }
    private void OnDestroy()
    {
        foreach (GameObject log in spawnedObjects)
        {
            Destroy(log);
        }
    }
}
