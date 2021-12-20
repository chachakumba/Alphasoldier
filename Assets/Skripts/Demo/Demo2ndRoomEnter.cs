using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2ndRoomEnter : MonoBehaviour
{
    [SerializeField] Logic[] turrets;
    [SerializeField] GameObject[] turretsPrefabs;
    Vector3[] turretsPos;
    [SerializeField] Door[] doors;
    [SerializeField] Spawner[] spawners;
    public float turretResetTime = 5;
    public float turretResetTimeRandAdder = 2;
    public int endPoint = 25;
    public int points = 0;
    bool continueSpawning = true;
    [SerializeField] Demo2ndRoomDisplay display;
    bool closed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!closed)
        {
            closed = true;
            Close();
            display.StartTalking(endPoint);
        }
    }
    void Close()
    {
        turretsPos = new Vector3[turrets.Length];
        for(int i = 0; i < turrets.Length; i++)
        {
            if (turrets[i] != null)
            {
                turretsPos[i] = turrets[i].transform.position;
                turrets[i].OnDeath += AddPoint;
                turrets[i].gameObject.SetActive(true);
            }
        }
        foreach (Spawner spawner in spawners) spawner.gameObject.SetActive(true);
        foreach (Door door in doors) door.CloseDoor();
    }
    IEnumerator ResetTurret(int id)
    {
        yield return new WaitForSeconds(turretResetTime + Random.Range(0f, turretResetTimeRandAdder));
        if (continueSpawning)
        {
            turrets[id] = Instantiate(turretsPrefabs[Random.Range(0, turretsPrefabs.Length)], turretsPos[id], Quaternion.identity).GetComponent<Logic>();
            turrets[id].OnDeath += AddPoint;
        }
    }
    void Open()
    {
        foreach (Spawner spawner in spawners) spawner.gameObject.SetActive(false);
        continueSpawning = false;
        doors[1].OpenDoor();
    }
    void AddPoint(object sender, System.EventArgs e)
    {
        points++;
        display.UpdateScore();
        if(points >= endPoint)
        {
            display.EndScoring();
            Open();
        }
        else
        {
            if (continueSpawning)
            {
                int id = -1;
                for (int i = 0; i < turrets.Length; i++)
                {
                    if (turrets[i] == sender as Logic) id = i;
                }
                StartCoroutine(ResetTurret(id));
            }
        }
    }
}
