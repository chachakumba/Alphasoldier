using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform shootPoint;
    public Sprite visual;
    public float damage;
    public float shootingSpeed = 0.5f;
    public float shakePower = 3;
    public float shakeDuration = 0.03f;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptyShootSound;
    protected AudioSource source;
    public float maxDistance;
    public float timeToFade = 0.2f;
    public AnimatorHumanoid animator;
    public Logic logic;
    public float recoil = 5;
    public int maxAmmo = 12;
    public int currentAmmo = 12;
    public bool reloading = false;
    public float reloadTime = 2;
    public event System.EventHandler OnReloadStart;
    public event System.EventHandler OnReloadEnd;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }
    public virtual void Shoot()
    {
    }
    public virtual void Reload()
    {
        InvokeReloadStart();
    }
    public void InvokeReloadStart()
    {
        OnReloadStart?.Invoke(this, System.EventArgs.Empty);
    }
    public void InvokeReloadEnd()
    {
        OnReloadEnd?.Invoke(this, System.EventArgs.Empty);
    }
}
