using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBotExplosion : MonoBehaviour
{
    public float speedOfExplosion;
    public float damage;
    public float maxRad;
    List<GameObject> hitted = new List<GameObject>();
    private void FixedUpdate()
    {
        transform.localScale += Vector3.one * speedOfExplosion;
        if (transform.localScale.x >= maxRad) Destroy(gameObject);
        Collider2D[] exploded = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x, Manager.instance.playerLayer + Manager.instance.enemyLayer);
        foreach (Collider2D exp in exploded)
        {
            RaycastHit2D[] hit = new RaycastHit2D[1];
            Physics2D.Raycast(transform.position, transform.position - exp.transform.position, Manager.instance.floorFilter, hit, Vector2.Distance(transform.position, exp.transform.position));
            if (exp.gameObject.GetComponent<IHaveHP>() != null && hit[0].collider == null)
            {
                if (hitted.Find(x => exp.gameObject) == null)
                {
                    exp.gameObject.GetComponent<IHaveHP>().GetDamage(damage);
                    hitted.Add(exp.gameObject);
                }
            }
        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void SetValues(float speedOfExplosion, float damage, float maxRad)
    {
        this.speedOfExplosion = speedOfExplosion;
        this.damage = damage;
        this.maxRad = maxRad;
    }
}
