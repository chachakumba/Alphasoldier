using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Veil : MonoBehaviour
{
    [SerializeField] bool destroyAfterUsed = false;
    [SerializeField] float speed = 0.1f;
    SpriteRenderer sprite;
    Coroutine cour;
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cour != null)
            StopCoroutine(cour);
        cour = StartCoroutine(FadeCour());
        onDisableEvent.Invoke();
    }
    IEnumerator FadeCour()
    {
        while (sprite.color.a > 0)
        {

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - speed);
            yield return new WaitForSeconds(0.1f);
        }
        if (destroyAfterUsed)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cour != null)
            StopCoroutine(cour);
        onEnableEvent.Invoke();
        cour = StartCoroutine(AppearCour());
    }
    IEnumerator AppearCour()
    {
        while (sprite.color.a < 1)
        {

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + speed);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
