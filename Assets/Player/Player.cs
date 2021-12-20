using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Player : Logic, IHaveHP
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider insideHealthSlider;
    [SerializeField, Range(0,1)]
    float healthSliderSpeed = 0.1f;
    public PlayerControls controls;
    Camera mainCam;
    Coroutine move;
    Coroutine shoot;
    Coroutine decreaceHealthSlider;
    Coroutine reload;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Image ammoImage;
    bool running = false;
    bool crouching = false;
    public bool canDie = true;
    protected override void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        mainCam = Manager.instance.mainCam;
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Shoot.performed += ctx => Shoot();
        controls.Player.Shoot.canceled += ctx => StopShoot();
        controls.Player.Run.performed += ctx => running = true;
        controls.Player.Run.canceled += ctx => running = false;
        controls.Player.Crouch.performed += ctx => crouching = true;
        controls.Player.Crouch.canceled += ctx => crouching = false;
        controls.Player.Reload.performed += ctx => Reload();
        weapon = animator.weapon;
        enemyLayer = Manager.instance.enemyLayer;
        enemyFilter.layerMask = enemyLayer;
        weapon.logic = this;
        Manager.instance.player = this;
        UpdateAmmoUI();
        weapon.OnReloadStart += ReloadUIStart;
        weapon.OnReloadEnd += ReloadUIEnd;
        base.Start();
    }
    protected override void Awake()
    {
        base.Awake();
        Manager.instance.player = this;
    }
    public override void ChangeSpeed()
    {
        if (running)
        {
            recoilMultiplier = runRecoilMultiplier;
            currentSpeed = speed * run * controls.Player.MoveHorizonal.ReadValue<float>() * animator.mirrored;
        }
        else if (crouching)
        {
            recoilMultiplier = crouchRecoilMultiplier;
            currentSpeed = speed * crouch * controls.Player.MoveHorizonal.ReadValue<float>() * animator.mirrored;
        }
        else
        {
            recoilMultiplier = baseRecoilMultiplier;
            currentSpeed = speed * controls.Player.MoveHorizonal.ReadValue<float>() * animator.mirrored;
        }
    }
    void Shoot()
    {
        if (shoot == null)
            shoot = StartCoroutine(IsShooting());
    }
    void StopShoot()
    {
        StopCoroutine(shoot);
        shoot = null;
    }
    IEnumerator IsShooting()
    {
        while (true)
        {
            weapon.Shoot();
            UpdateAmmoUI();
            yield return new WaitForSeconds(weapon.shootingSpeed);
        }
    }
    private void FixedUpdate()
    {
        LookOnMouse(mainCam.ScreenToWorldPoint(controls.Player.MousePos.ReadValue<Vector2>()));
        ChangeSpeed();
        if (controls.Player.MoveHorizonal.ReadValue<float>() != 0) Move();

        if (healthSlider.value < insideHealthSlider.value)
        {
            insideHealthSlider.value -= healthSliderSpeed;
        }

        if (SaveManager.save != null)
            SaveManager.save.playTime += Time.deltaTime;
    }
    void LookOnMouse(Vector2 mouseDelta)
    {
        animator.Look(mouseDelta);
    }
    public void Move()
    {
        transform.Translate(Vector2.right * currentSpeed * speedMultiplier * animator.mirrored);
    }
    public void Jump()
    {
        if (animator.jumps < animator.maxJumps && animator.grounded)
        {
            animator.jumps++;
            rb.AddForce(Vector2.up * jumpPower * jumpMultiplier, ForceMode2D.Impulse);
        }
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public void GetDamage(float damage)
    {
        insideHealthSlider.value = health / maxHealth;
        health -= damage;
        UpdateHealthUI();
        if (health <= 0) Die();
    }
    public void Heal(float amount)
    {
        health += amount;
        UpdateHealthUI();
        if (health > maxHealth) health = maxHealth;
    }
    void Reload()
    {
        if (!weapon.reloading && weapon.currentAmmo < weapon.maxAmmo)
        {
            weapon.Reload();
        }
    }
    void ReloadUIStart(object sender, EventArgs e)
    {
        ammoText.color = Color.grey;
        ammoImage.gameObject.SetActive(true);
        ammoText.text = weapon.currentAmmo + "/" + weapon.maxAmmo;
    }
    void ReloadUIEnd(object sender, EventArgs e)
    {
        ammoText.color = Color.white;
        ammoImage.gameObject.SetActive(false);
        ammoText.text = weapon.currentAmmo + "/" + weapon.maxAmmo;
    }
    public void UpdateAmmoUI()
    {
        ammoText.text = weapon.currentAmmo + "/" + weapon.maxAmmo;
    }
    public void UpdateHealthUI()
    {
        healthSlider.value = health / maxHealth;
    }
    public void Die()
    {
        if (canDie)
            Manager.instance.PlayerDeath();
    }
}
