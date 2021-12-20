using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrorist : Logic, IHaveHP
{
    public float timeToAttack = 0.5f;
    public float timeToLookForAttack = 1;
    public float foundRadius = 7;
    public float foundMaxRadius = 15;
    public float searchTime = 10;
    Coroutine currentState;
    protected override void Start()
    {
        enemyLayer = Manager.instance.playerLayer;
        enemyFilter = Manager.instance.playerFilter;
        base.Start();
    }
    public void ChangeStateTerr(State newState)
    {
        StopCoroutine(currentState);
        state = newState;
        switch (state){
            case State.idle:
                currentState = StartCoroutine(Idle());
                break;
            case State.search:
                currentState = StartCoroutine(Search());
                break;
            case State.attack:
                currentState = StartCoroutine(Attack());
                break;
        }
    }
    IEnumerator Idle()
    {
        yield return null;
        while (state == State.idle)
        {
            yield return null;
        }
    }
    IEnumerator Search()
    {
        yield return null;
        float timeToLoseTarget = Time.time + searchTime;
        while (timeToLoseTarget > Time.time)
        {
            Move(Manager.instance.lastPlayerFoundPosition);
            RaycastHit2D[] circleHits = Physics2D.CircleCastAll(transform.position, foundRadius, transform.forward, enemyLayer);
            if(circleHits.Length > 0)
            {
                foreach (RaycastHit2D targ in circleHits)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, foundMaxRadius, enemyLayer);
                    if (!hit.collider.CompareTag("Wall"))
                    {
                        Manager.instance.lastPlayerFoundPosition = hit.collider.transform.position;
                        ChangeStateTerr(State.attack);
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        ChangeStateTerr(State.idle);
    }
    IEnumerator Attack()
    {
        yield return null;
        Manager.instance.lastPlayerFoundPosition = Manager.instance.player.transform.position;
        while(state == State.attack)
        {
            yield return new WaitForSeconds(timeToLookForAttack);
            RaycastHit2D hit = Physics2D.Raycast(weapon.shootPoint.position, weapon.shootPoint.forward, foundMaxRadius, enemyLayer + Manager.instance.floorLayer);
            if (hit)
            {
                yield return new WaitForSeconds(timeToAttack);
                weapon.Shoot();
            }
            else
            {
                ChangeState(State.search);
            }
            yield return null;
        }
    }
    public void Move(Vector2 dest)
    {
        
    }
    public void GetDamage(float damage)
    {
        health -= damage;
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
        Destroy(gameObject);
    }
}
