using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOnStep : MonoBehaviour
{
    Collider2D trigger;
    [SerializeField] float speedOfMove = 1;
    ContactFilter2D filter;
    Coroutine cour;
    AudioSource source;
    [SerializeField] AudioClip activeSound;
    public float minHeigth;
    public float addHeigth = 5;
    private void Start()
    {
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            if (col.isTrigger) trigger = col;
        }
        source = GetComponent<AudioSource>();
        filter.useLayerMask = true;
        filter.layerMask = Manager.instance.entityLayer;
        minHeigth = transform.position.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Entity"))
        {
            MoveElevator(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Entity"))
        {
            if (trigger.Cast(Vector2.zero, filter, new RaycastHit2D[1]) < 1)
            {
                MoveElevator(false);
            }
        }
    }
    void MoveElevator(bool move)
    {
        if (cour != null) StopCoroutine(cour);
        if (move)
        {
            cour = StartCoroutine(ManipulateDoorCour(speedOfMove * 0.05f));
        }
        else
        {
            cour = StartCoroutine(ManipulateDoorCour(speedOfMove * -0.05f));
        }
    }
    IEnumerator ManipulateDoorCour(float addition)
    {
        if (!source.isPlaying)
        {
            source.clip = activeSound;
            source.Play();
        }
        while (transform.position.y <= minHeigth + addHeigth && transform.position.y >= minHeigth)
        {
            transform.position += Vector3.up * addition;
            yield return new WaitForSeconds(0.01f);
        }
        if (transform.localPosition.y > minHeigth + addHeigth) transform.position = new Vector3(transform.position.x, (minHeigth + addHeigth), 0);
        if (transform.localPosition.y < minHeigth) transform.position = new Vector3(transform.position.x, minHeigth, 0);
        source.clip = null;
        source.Stop();
    }
}
