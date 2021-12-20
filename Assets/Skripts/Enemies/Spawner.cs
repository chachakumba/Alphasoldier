using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnedPrefab;
    [SerializeField] Transform spawnPoint;
    List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField] float spawnTime = 8;
    float time = 0;
    private void Update()
    {
        if(time < Time.time)
        {
            time = Time.time + spawnTime;
            spawnedObjects.Add(Instantiate(spawnedPrefab, spawnPoint.position, Quaternion.identity));
            spawnedObjects[spawnedObjects.Count - 1].GetComponent<Logic>().OnDeath += RemoveFromArray;
        }
    }
    void RemoveFromArray(object obj, System.EventArgs e)
    {
        spawnedObjects.Remove(obj as GameObject);
    }
    private void OnDestroy()
    {
        foreach(GameObject log in spawnedObjects)
        {
            Destroy(log);
        }
    }
}
