using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemoDownTrapper : MonoBehaviour
{
    bool wasClosed = false;
    [SerializeField] WallDestroyable wall;
    [SerializeField] GameObject spawner;
    [SerializeField] Door door;
    private void Awake()
    {
        wall.OnDeath += Open;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasClosed)
        {
            wasClosed = true;
            Close();
        }
    }
    void Open(object send, System.EventArgs args)
    {
        door.OpenDoor();
        spawner.SetActive(false);
        Destroy(gameObject);
    }
    void Close()
    {
        door.CloseDoor();
        spawner.SetActive(true);
    }
}
