using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTurret : Boss, IHaveHP
{
    public float timeToAttack = 1f;
    public float timeToLookForAttack = 2;
    public Transform weaponPos;
    public SpriteRenderer stateBulb;
    public Color[] angerColors;
    public float maxRotGrad = 45;
    public float rotSpeed = 1;
    [SerializeField] GameObject hpDrop;
    public int shotsAmount = 15;
    public float timeBetweenShots = 0.1f;
    public float recoil = 10;
    [Space]
    [SerializeField] BossTurretMinion[] minions;
    public int angerLvl = 1;
    [SerializeField] float waitTillRepair = 10;
    [SerializeField] float timeTillAttackSubtract = 0.1f;
    [SerializeField] float timeToLookForAttackSubtract = 0.25f;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider insideHealthBar;
    Transform playerPos;
    Coroutine decreaceHealthSlider;
    [SerializeField] float healthSliderSpeed = 0.1f;
    [SerializeField] AudioClip[] phasesClpis;
    bool[] phaseClipsPlayed;
    [SerializeField] AudioClip allUnitsSpawnClip;
    [SerializeField] AudioClip unitSpawnClip;
    public GameObject spawnParticles;
    float savedRot = 0;
    int shotNum = 0;
    float timeToCharge = 0;
    [SerializeField] AudioSource bossAudio;
    [SerializeField ]float timeToAudioFade = 1;
    protected override void Start()
    {
        enemyLayer = Manager.instance.playerLayer;
        enemyFilter.layerMask = enemyLayer;
        base.Start();
        playerPos = Manager.instance.player.transform;
        stateBulb.color = angerColors[0];
    }

    protected override void Awake()
    {
        phaseClipsPlayed = new bool[phasesClpis.Length];
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        foreach (BossTurretMinion minion in minions)
        {
            minion.boss = this;
        }
    }
    void Update()
    {
        if (timeToWork <= Time.time)
        {
            switch (state)
            {
                case State.prepareAttack:
                    LookAtTarget();
                    if (timeToCharge < Time.time)
                    {
                        if (!preparedAttack)
                        {
                            preparedAttack = true;
                            timeToCharge = Time.time + timeToLookForAttack - timeToLookForAttackSubtract * angerLvl;
                        }
                        else
                        {
                            preparedAttack = false;
                            state = State.chargeAttack;
                        }
                    }
                    break;
                case State.chargeAttack:
                    shotNum = shotsAmount;
                    state = State.attack;
                    timeToWork = Time.time + timeToAttack - timeTillAttackSubtract * angerLvl;
                    break;
                case State.attack:
                    if (shotNum > 0)
                    {
                        if(shotNum == shotsAmount) savedRot = weaponPos.transform.eulerAngles.z;
                        shotNum--;
                        weapon.Shoot();
                        float recoilGrad = Random.Range(-recoil, recoil);
                        weaponPos.eulerAngles = new Vector3(0, 0, savedRot + recoilGrad);
                        timeToWork = Time.time + timeBetweenShots;
                    }
                    else
                    {
                        state = State.prepareAttack;
                    }
                    break;
                case State.death:
                    break;
                default:
                    Debug.Log("default");
                    state = State.prepareAttack;
                    break;
            }
        }
    }
    public override void ActivateBoss()
    {
        healthBar.gameObject.SetActive(true);
        bossAudio.Play();
    }
    void LookAtTarget()
    {
        Manager.instance.lastPlayerFoundPosition = playerPos.position;
        weaponPos.up = -(new Vector3(Manager.instance.lastPlayerFoundPosition.x, Manager.instance.lastPlayerFoundPosition.y, 0) - weaponPos.position);
    }
    void RepairAllTurrets()
    {
        foreach (BossTurretMinion minion in minions)
        {
            if (!minion.gameObject.activeSelf)
            {
                minion.health = minion.maxHealth;
                minion.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator CheckAllTurrets()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTillRepair);
            RepairAllTurrets();
        }
    }
    public void RepairOneTurret(BossTurretMinion damaged)
    {
        StartCoroutine(RepairOneTurretCour(damaged));
    }
    IEnumerator RepairOneTurretCour(BossTurretMinion damaged)
    {
        yield return new WaitForSeconds(waitTillRepair/2);
        foreach (BossTurretMinion minion in minions)
        {
            if (minion == damaged && !minion.gameObject.activeSelf)
            {
                minion.health = minion.maxHealth;
                minion.gameObject.SetActive(true);
            }
        }
    }
    public void GetDamage(float damage)
    {
        insideHealthBar.value = health / maxHealth;
        health -= damage;
        //healthBar.value = health / maxHealth;
        if (health <= 0)
            Die();
        else if (health / maxHealth <= 0.1f)
        {
            stateBulb.color = angerColors[4];
            if (!phaseClipsPlayed[3])
            {
                phaseClipsPlayed[3] = true;
                Manager.instance.PlaySound(transform.position, phasesClpis[3]);
            }
            Debug.LogWarning("5 phase");
            angerLvl = 5;
        }
        else if (health / maxHealth <= 0.25f)
        {
            Debug.LogWarning("4 phase");
            stateBulb.color = angerColors[3];
            if (!phaseClipsPlayed[2])
            {
                phaseClipsPlayed[2] = true;
                Manager.instance.PlaySound(transform.position, phasesClpis[2]);
            }
            RepairAllTurrets();
            StartCoroutine(CheckAllTurrets());
            angerLvl = 4;
        }
        else if (health / maxHealth <= 0.5f)
        {
            Debug.LogWarning("3 phase");
            stateBulb.color = angerColors[2];
            if (!phaseClipsPlayed[1])
            {
                phaseClipsPlayed[1] = true;
                Manager.instance.PlaySound(transform.position, phasesClpis[1]);
            }
            StartCoroutine(CheckAllTurrets());
            angerLvl = 3;
        }
        else if (health / maxHealth <= 0.75f)
        {
            Debug.LogWarning("2 phase");
            stateBulb.color = angerColors[1];
            if (!phaseClipsPlayed[0])
            {
                phaseClipsPlayed[0] = true;
                Manager.instance.PlaySound(transform.position, phasesClpis[0]);
            }
            RepairAllTurrets();
            angerLvl = 2;
        }
        healthBar.value = health / maxHealth;
        if (decreaceHealthSlider == null && gameObject.activeSelf) StartCoroutine(DecreaceHealthSlider());
    }
    IEnumerator DecreaceHealthSlider()
    {
        while (healthBar.value < insideHealthBar.value)
        {
            insideHealthBar.value -= healthSliderSpeed;
            yield return new WaitForSeconds(0.1f);
        }
        decreaceHealthSlider = null;
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
    public void Die()
    {
        InvokeDeath(this, System.EventArgs.Empty);
        int rand = Random.Range(0, 100);
        if (rand < 30)
        {
        }
        else if (rand < 90)
        {
            Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        foreach(BossTurretMinion minion in minions)
        {
            minion.Die();
        }
        Manager.instance.PlaySound(transform.position, deathSound);
        Manager.instance.FadeAudio(bossAudio, 0.1f);
        Manager.instance.ShakeSkreen(3, 3);
        Destroy(gameObject);
    }
}
public class BossTurretMinion : Logic, IHaveHP
{
    public BossTurret boss;

    public float timeToAttack = 0.5f;
    public float randTimeToAttackAdd = 0.5f;
    public float timeToLookForAttack = 1;
    public float randTimeToLookForAttackAdd = 0.5f;
    public Transform weaponPos;
    public SpriteRenderer stateBulb;
    public Color searchColor = Color.yellow;
    public Color attackColor = Color.red;
    public float maxRotGrad = 45;
    public float rotSpeed = 1;
    [SerializeField] protected GameObject hpDrop;
    public int shotsAmount = 10;
    public float timeBetweenShots = 0.1f;
    public float recoil = 10;
    protected Transform playerPos;
    [SerializeField] float awakeTime = 3;
    [SerializeField] float randAwakeAdd = 2;
    protected float savedRot = 0;
    protected int shotNum = 0;
    protected float timeToCharge = 0;
    [SerializeField] GameObject spawnParticle;
    protected override void Start()
    {
        enemyLayer = Manager.instance.playerLayer;
        enemyFilter.layerMask = enemyLayer;
        base.Start();
        playerPos = Manager.instance.player.transform;
    }

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        weapon.logic = this;
    }
    private void OnEnable()
    {
        StartCoroutine(SpawnSpawnParticles());
        timeToWork = Time.time + awakeTime + Random.Range(0, randAwakeAdd);
        Manager.instance.PlaySound(transform.position, buildSound);
    }
    void Update()
    {
        if (timeToWork <= Time.time)
        {
            switch (state)
            {
                case State.prepareAttack:
                    LookAtTarget();
                    if (timeToCharge < Time.time)
                    {
                        if (!preparedAttack)
                        {
                            preparedAttack = true;
                            timeToCharge = Time.time + timeToLookForAttack + Random.Range(0, randTimeToLookForAttackAdd);
                        }
                        else
                        {
                            preparedAttack = false;
                            state = State.chargeAttack;
                            Manager.instance.PlaySound(transform.position, findTargetSound);
                        }
                    }
                    break;
                case State.chargeAttack:
                    shotNum = shotsAmount;
                    state = State.attack;
                    timeToWork = Time.time + timeToAttack + Random.Range(0, randTimeToAttackAdd);
                    break;
                case State.attack:
                    Attack();
                    break;
                default:
                    Debug.Log("default");
                    state = State.prepareAttack;
                    break;
            }
        }
    }
    IEnumerator SpawnSpawnParticles()
    {
        spawnParticle = Instantiate(boss.spawnParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);
        Destroy(spawnParticle);
    }
    protected virtual void Attack()
    {
        if (shotNum > 0)
        {
            if (shotNum == shotsAmount) savedRot = weaponPos.transform.eulerAngles.z;
            shotNum--;
            weapon.Shoot();
            float recoilGrad = Random.Range(-recoil, recoil);
            weaponPos.eulerAngles = new Vector3(0, 0, savedRot + recoilGrad);
            timeToWork = Time.time + timeBetweenShots;
        }
        else
        {
            state = State.prepareAttack;
        }
    }
    protected void LookAtTarget()
    {
        Manager.instance.lastPlayerFoundPosition = playerPos.position;
        weaponPos.up = -(new Vector3(Manager.instance.lastPlayerFoundPosition.x, Manager.instance.lastPlayerFoundPosition.y, 0) - weaponPos.position);
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
        int rand = Random.Range(0, 100);
        if (rand < 30)
        {
            for (int i = 0; i < 2; i++)
                Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        else if (rand < 90)
        {
            for (int i = 0; i < 3; i++)
                Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        else
        {
            for (int i = 0; i < 5; i++)
                Instantiate(hpDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        StopAllCoroutines();

        rand = Random.Range(0, 100);
        if (rand < 10)
            boss.RepairOneTurret(this);

        if (spawnParticle!= null)
            Destroy(spawnParticle);

        gameObject.SetActive(false);
    }
}