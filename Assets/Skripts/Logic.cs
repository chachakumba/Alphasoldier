using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Logic
{
    public virtual void ActivateBoss()
    {

    }
}
public class Logic : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;
    public float speed = 5;
    public float currentSpeed = 5;
    public float run = 1.5f;
    public float runRecoilMultiplier = 2;
    public float crouch = 0.25f;
    public float crouchRecoilMultiplier = 0.5f;
    public float baseRecoilMultiplier = 1;
    public float recoilMultiplier = 1;
    public float speedMultiplier = 10;
    public float jumpPower = 1;
    public float jumpMultiplier = 30;
    public Weapon weapon;
    public LayerMask enemyLayer;
    public ContactFilter2D enemyFilter;
    public ContactFilter2D floorAndEnemyFilter;
    protected Rigidbody2D rb;
    protected AnimatorHumanoid animator;
    public event System.EventHandler OnDeath;
    protected float timeToWork = 0;
    protected State state;
    protected bool preparedAttack = false;
    bool running = false;
    bool crouching = false;
    [SerializeField] protected AudioClip buildSound;
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip findTargetSound;
    [SerializeField] protected AudioClip loseTargetSound;
    public List<DropManager> drops;
    protected virtual void Awake()
    {
        animator = GetComponent<AnimatorHumanoid>();
        animator.logic = this;
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        currentSpeed = speed;
        enemyFilter.useLayerMask = true;
        recoilMultiplier = baseRecoilMultiplier;
    }
    public virtual void ChangeSpeed()
    {
        if (running)
        {
            recoilMultiplier = runRecoilMultiplier;
            currentSpeed = speed * run * animator.mirrored;
        }
        else if (crouching)
        {
            recoilMultiplier = crouchRecoilMultiplier;
            currentSpeed = speed * crouch * animator.mirrored;
        }
        else
        {
            recoilMultiplier = baseRecoilMultiplier;
            currentSpeed = speed * animator.mirrored;
        }
    }
    protected virtual void Start()
    {
        floorAndEnemyFilter.layerMask = enemyFilter.layerMask + Manager.instance.floorLayer;
        floorAndEnemyFilter.useLayerMask = true;
    }
    public void InvokeDeath(object sender, System.EventArgs args)
    {
        OnDeath?.Invoke(sender, args);
    }
    public void ChangeState(State newState)
    {
        state = newState;
    }
    public void AddWaitTime(float addition)
    {
        timeToWork += addition;
    }
    public void SetWaitTime(float addition)
    {
        timeToWork = Time.time + addition;
    }
    public void Drop()
    {
        foreach (DropManager drop in drops)
        {
            if(Random.Range(0,100) >= drop.chance)
            {
                foreach (GameObject dropObj in drop.drop)
                {
                    Instantiate(dropObj, transform.position, Quaternion.identity);
                }
                break;
            }
        }
    }
}

public interface IHaveHP
{
    void GetDamage(float damage);
    void Heal(float amount);
}
public enum State { idle, search, prepareAttack, chargeAttack, attack, death }
[System.Serializable]
public class DropManager
{
    [Range(0,100)]
    public float chance = 100;
    public GameObject[] drop;
}