using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBulletHitGetter : MonoBehaviour, IHaveHP
{
    TrapBullet parentObj;
    private void Awake()
    {
        parentObj = GetComponentInParent<TrapBullet>();
    }
    public void GetDamage(float amount)
    {
        Destroy(parentObj.gameObject);
    }
    public void Heal(float amount)
    {

    }
}
