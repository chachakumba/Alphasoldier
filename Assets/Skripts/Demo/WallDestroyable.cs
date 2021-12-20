using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyable : MonoBehaviour, IHaveHP
{
    float health = 1000;
    [SerializeField] float maxHealth = 1000;
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject deathParticles;
    [SerializeField] GameObject[] cracks;
    public event System.EventHandler OnDeath;
    private void Awake()
    {
        health = maxHealth;
    }
    public void GetDamage(float amount)
    {
        health -= amount;
        particles.Play();
        CheckCracks();
        if (health <= 0)
        {
            OnDeath?.Invoke(this, System.EventArgs.Empty);
            Destroy(Instantiate(particles, transform.position, transform.rotation), 2);
            Destroy(gameObject);
        }
    }
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        CheckCracks();
    }
    void CheckCracks()
    {
        float part = maxHealth / cracks.Length;
        if (health < part * 1)
        {
            cracks[0].SetActive(true);
            cracks[1].SetActive(true);
            cracks[2].SetActive(true);
        }
        else if (health < part * 2)
        {
            cracks[0].SetActive(true);
            cracks[1].SetActive(true);
        }
        else if (health < part * 3)
        {
            cracks[0].SetActive(true);
        }
    }
}
