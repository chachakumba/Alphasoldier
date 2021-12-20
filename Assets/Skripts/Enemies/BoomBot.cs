using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBot : Logic, IHaveHP
{
    [SerializeField] GameObject hpDrop;
    [SerializeField] float maxXVelocity = 25;
    [SerializeField] float activateExplodeRad = 2;
    [SerializeField] float explodeRad = 2.5f;
    [SerializeField] float explodeDamage = 30;
    [SerializeField] float lifeTime = 10;
    [SerializeField] float timeToExplode = 0.5f;
    [SerializeField] Transform visuals;
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelsRot = 5;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionSpeed = 10;
    Logic player;
    protected override void Awake()
    {
        state = State.search;
        rb = GetComponent<Rigidbody2D>();
        player = Manager.instance.player;
        Destroy(gameObject, lifeTime);
    }
    protected override void Start()
    {
        base.Start();
        player = Manager.instance.player;
    }

    void FixedUpdate()
    {
        if (timeToWork <= Time.time)
        {
            switch (state)
            {
                case State.search:
                    if(Vector2.Distance(player.transform.position, transform.position) <= activateExplodeRad)
                    {
                        Collider2D[] exploded = Physics2D.OverlapCircleAll(transform.position, explodeRad, Manager.instance.playerLayer + Manager.instance.enemyLayer);
                        foreach (Collider2D exp in exploded)
                        {
                            RaycastHit2D[] hit = new RaycastHit2D[1];
                            Physics2D.Raycast(transform.position, transform.position - exp.transform.position, Manager.instance.floorFilter, hit, Vector2.Distance(transform.position, exp.transform.position));

                            if (exp.gameObject.GetComponent<IHaveHP>() != null && hit[0].collider == null)
                            {
                                state = State.attack;
                            }
                        }
                        break;
                    }

                    if(player.transform.position.x > transform.position.x)
                    {
                        rb.AddForce(Vector2.right * speed * speedMultiplier);
                        visuals.transform.localEulerAngles = new Vector3(0, 180, 0);
                        foreach (Transform wheel in wheels) wheel.localEulerAngles += Vector3.forward * wheelsRot * Mathf.Abs(rb.velocity.x);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * speed * speedMultiplier);
                        visuals.transform.localEulerAngles = new Vector3(0, 0, 0);
                        foreach (Transform wheel in wheels) wheel.localEulerAngles += Vector3.forward * wheelsRot * Mathf.Abs(rb.velocity.x);
                    }

                    if(rb.velocity.x > maxXVelocity)
                    {
                        rb.velocity = new Vector2(maxXVelocity, rb.velocity.y);
                    }
                    else if (rb.velocity.x < -maxXVelocity)
                    {
                        rb.velocity = new Vector2(-maxXVelocity, rb.velocity.y);
                    }
                    break;
                case State.attack:
                    StartCoroutine(Explode());
                    break;
                default:
                    state = State.search;
                    break;
            }
        }
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(timeToExplode);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        InvokeDeath(this, System.EventArgs.Empty);
        Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<BoomBotExplosion>().SetValues(explosionSpeed, explodeDamage, explodeRad);
    }
    public void GetDamage(float damage)
    {
        health -= damage;
        state = State.prepareAttack;
        if (health <= 0)
            Die();
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
    public void Die()
    {
        int rand = Random.Range(0, 100);
        if (rand < 60)
        {
        }
        else
        {
            Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
