using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector2 startForceAdder = new Vector2(10, 10);
    protected Rigidbody2D rb;
    public float searchingRadius = 5;
    public float forceMultiplier = 5;
    public float nearDist = 1;
    public float fadeTime = 10;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected void Start()
    {
        rb.AddForce(new Vector2(Random.Range(-1f, 1f) * startForceAdder.x, startForceAdder.y * Random.Range(0.9f, 1.2f)), ForceMode2D.Impulse);
        Destroy(gameObject, fadeTime);
    }
    private void FixedUpdate()
    {
        if (Time.deltaTime > 0)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, searchingRadius, Manager.instance.playerLayer);
            foreach (Collider2D col in cols)
            {
                if (col.GetComponent<Player>() != null)
                {
                    if (Vector2.Distance(transform.position, col.transform.position) <= nearDist)
                        Collect(col.GetComponent<Player>());
                    else
                    {
                        rb.AddForce(new Vector2(col.transform.position.x - transform.position.x, col.transform.position.y - transform.position.y).normalized * forceMultiplier);
                    }
                }
            }
        }
    }
    protected virtual void Collect(Player player)
    {
        Destroy(gameObject);
    }
}
