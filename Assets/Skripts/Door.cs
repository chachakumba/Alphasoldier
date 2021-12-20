using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Collider2D trigger;
    [SerializeField] Collider2D doorPhys;
    [SerializeField] float speedOfVisOpen = 1;
    [SerializeField] bool isInstant = true;
    [SerializeField] float heigth;
    ContactFilter2D filter;
    AudioSource source;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    public bool closed = false;
    [SerializeField] SpriteRenderer stateBulb;
    [SerializeField] Color[] bulbColors;
    float addition = 0;
    [SerializeField] bool checkPermaLock = false;
    private void Start()
    {
        trigger = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();
        filter.useLayerMask = true;
        filter.layerMask = Manager.instance.entityLayer;

        if (closed)
            stateBulb.color = bulbColors[2];
        else
            stateBulb.color = bulbColors[1];
    }
    public void CloseDoor()
    {
        closed = true;
        ManipulateDoor(false);
    }
    public void OpenDoor()
    {
        closed = false;
        stateBulb.color = bulbColors[1];
    }
    private void FixedUpdate()
    {
        if (doorPhys.transform.localPosition.y <= heigth && doorPhys.transform.localPosition.y >= 0)
        {
            doorPhys.transform.position += Vector3.up * addition;
        }
        else if (doorPhys.transform.localPosition.y > heigth)
        {
            addition = 0;
            doorPhys.transform.localPosition = Vector3.up * heigth;
        }
        else if (doorPhys.transform.localPosition.y < 0)
        {
            addition = 0;
            doorPhys.transform.localPosition = Vector3.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Wall") && !collision.CompareTag("Particle") && !collision.CompareTag("Loot") && !closed)
        {
            ManipulateDoor(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Wall") && !collision.CompareTag("Particle") && !collision.CompareTag("Loot") && !closed)
        {
            if(trigger.Cast(Vector2.zero, filter, new RaycastHit2D [1]) < 1)
            {
                ManipulateDoor(false);
            }
        }
    }
    void ManipulateDoor(bool open)
    {
        if (open)
        {
            stateBulb.color = bulbColors[0];
            source.clip = openSound;
            source.Play();
            if (isInstant) doorPhys.enabled = false;
            addition = speedOfVisOpen * 0.05f;
        }
        else
        {
            if(closed)
                stateBulb.color = bulbColors[2];
            else
                stateBulb.color = bulbColors[1];
            if (source != null)
            {
                source.clip = closeSound;
                source.Play();
            }
            if (isInstant) doorPhys.enabled = true;
            addition = speedOfVisOpen * -0.05f;
        }
    }
}
