using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHealth : Loot
{
    public float amount = 20;
    protected override void Collect(Player player)
    {
        Manager.instance.player.GetComponent<IHaveHP>().Heal(amount);
        Destroy(gameObject);
    }
}
