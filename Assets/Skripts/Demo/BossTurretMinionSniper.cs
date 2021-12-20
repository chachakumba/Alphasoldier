using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretMinionSniper : BossTurretMinion
{
    protected override void Attack()
    {
        weapon.Shoot();
        timeToWork = Time.time + timeBetweenShots;
        state = State.prepareAttack;
    }
}