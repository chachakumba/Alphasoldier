using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DeathWall : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float speedAdder = 0.05f;
    Rigidbody2D rb;
    [SerializeField] float maxX = 0;
    Transform player;
    [SerializeField] float playerMaxDist = 15;
    [SerializeField] SpriteRenderer bulb;
    [SerializeField] Color normalColor = Color.yellow;
    [SerializeField] Color angryColor = Color.red;
    [SerializeField] Color deactivatedColor = Color.black;
    private void Start()
    {
        player = Manager.instance.player.transform;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (player.position.x - transform.position.x > playerMaxDist)
        {
            bulb.color = angryColor;
            speed += speedAdder;
        }
        else
        {
            bulb.color = normalColor;
        }

        if (maxX > transform.position.x)
        {
            rb.velocity = Vector3.right * speed;
        }
        else
        {
            transform.position = new Vector3(maxX, transform.position.y, 0);
            enabled = false;
            bulb.color = deactivatedColor;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Manager.instance.PlayerDeath();
        }
    }
}
