using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Logic, IHaveHP
{
    public float timeToAttack = 0.5f;
    public float timeToLookForAttack = 1;
    public float foundRadius = 15;
    public float foundMaxRadius = 25;
    public Transform weaponPos;
    public SpriteRenderer stateBulb;
    public Color searchColor = Color.yellow;
    public Color attackColor = Color.red;
    public float maxRotGrad = 45;
    public float rotSpeed = 1;
    [SerializeField] GameObject hpDrop;
    float timeToCharge = 0;
    bool moveLeft = true;
    [SerializeField] GameObject buildObject;
    [SerializeField] float awakeTime = 1;
    [SerializeField] float randAwakeAdd = 0.5f;
    protected override void Start()
    {
        enemyLayer = Manager.instance.playerLayer;
        enemyFilter.layerMask = enemyLayer;
        base.Start();
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }
    private void OnEnable()
    {
        if (Time.time > 1)
        {
            timeToWork = Time.time + awakeTime + Random.Range(0, randAwakeAdd);
            Destroy(Instantiate(buildObject, transform.position, Quaternion.identity), timeToWork - Time.time);
            Manager.instance.PlaySound(transform.position, buildSound);
        }
    }

    void Update()
    {
        if (timeToWork <= Time.time)
        {
            switch (state)
            {
                case State.search:
                    WeaponMove();
                    RaycastHit2D[] circleHits = Physics2D.CircleCastAll(transform.position, foundRadius, transform.forward, foundRadius, enemyLayer);
                    if (circleHits.Length > 0)
                    {
                        foreach (RaycastHit2D targ in circleHits)
                        {
                            RaycastHit2D[] hit = new RaycastHit2D[1];
                            if (Physics2D.Linecast(transform.position, targ.collider.transform.position, floorAndEnemyFilter, hit) > 0)
                            {
                                if (!hit[0].collider.CompareTag("Wall"))
                                {
                                    Manager.instance.lastPlayerFoundPosition = hit[0].collider.transform.position;
                                    state = State.prepareAttack;
                                    Manager.instance.PlaySound(transform.position, findTargetSound);
                                }
                            }
                        }
                    }
                    break;
                case State.prepareAttack:
                    LookAtTarget();
                    if (timeToCharge < Time.time)
                    {
                        if (!preparedAttack)
                        {
                            preparedAttack = true;
                            timeToCharge = Time.time + timeToLookForAttack;
                        }
                        else
                        {
                            RaycastHit2D[] hit = new RaycastHit2D[1];
                            if (Physics2D.Linecast(transform.position, Manager.instance.player.transform.position, floorAndEnemyFilter, hit) > 0)
                            {
                                if (!hit[0].collider.CompareTag("Wall"))
                                {
                                    preparedAttack = false;
                                    state = State.chargeAttack;
                                }
                                else
                                {
                                    state = State.search;
                                    Manager.instance.PlaySound(transform.position, loseTargetSound);
                                }
                            }
                            else
                            {
                                state = State.search;
                                Manager.instance.PlaySound(transform.position, loseTargetSound);
                            }
                        }
                    }
                    break;
                case State.chargeAttack:
                    state = State.attack;
                    timeToWork = Time.time + timeToAttack;
                    break;
                case State.attack:
                    weapon.Shoot();
                    timeToWork = Time.time + weapon.reloadTime;
                    state = State.prepareAttack;
                    break;
                default:
                    Debug.Log("default");
                    state = State.prepareAttack;
                    break;
            }
        }
    }
    void WeaponMove()
    {
        float rotation = weaponPos.rotation.z;
        if (moveLeft)
        {
            rotation -= rotSpeed;
            weaponPos.rotation = Quaternion.Euler(0, 0, rotation);
            if (rotation < -maxRotGrad) moveLeft = false;
        }
        else
        {
            rotation += rotSpeed;
            weaponPos.rotation = Quaternion.Euler(0, 0, rotation);
            if (rotation > maxRotGrad) moveLeft = true;
        }
    }
    void LookAtTarget()
    {
        if (Vector2.Distance(transform.position, Manager.instance.player.transform.position) < foundMaxRadius)
        {
            Manager.instance.lastPlayerFoundPosition = Manager.instance.player.transform.position;
            weaponPos.up = -(new Vector3(Manager.instance.lastPlayerFoundPosition.x, Manager.instance.lastPlayerFoundPosition.y, 0) - weaponPos.position);
        }
        else
        {
            state = State.search;
        }
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
        InvokeDeath(this, System.EventArgs.Empty);
        Drop();
        Destroy(gameObject);
    }
}
