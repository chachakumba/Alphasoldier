using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    public event System.EventHandler OnDestroy;
    public float damage = 10;
    public float speed = 1;
    public float lifeTime = 1;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.GetComponent<IHaveHP>() != null)
                collision.GetComponent<IHaveHP>().GetDamage(damage);
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        OnDestroy?.Invoke(this, System.EventArgs.Empty);
    }
}
