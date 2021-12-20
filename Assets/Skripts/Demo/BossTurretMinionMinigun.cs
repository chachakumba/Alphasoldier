using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretMinionMinigun : BossTurretMinion
{
    protected override void Attack()
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
}
