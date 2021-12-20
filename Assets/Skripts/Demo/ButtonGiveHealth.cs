using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGiveHealth : MonoBehaviour
{
    [SerializeField] bool healsToFull = false;
    public int healAmount = 100;
    [SerializeField] Button button;
    [SerializeField] float timeOfLight = 0.5f;
    [SerializeField] SpriteRenderer bulb;
    [SerializeField] Color openColor = Color.green;
    [SerializeField] Color closedColor = Color.red;
    [SerializeField] AudioClip use;
    [SerializeField] AudioClip deny;
    bool canUse = true;
    void Awake()
    {
        button.OnPress += OnChange;
        bulb.color = openColor;
    }

    private void OnChange(bool state)
    {
        if (canUse)
        {
            if (button.oneTimePressable)
            {
                canUse = false;
                bulb.color = closedColor;
            }
            else
                StartCoroutine(LightBulb());
            if (healsToFull)
            {
                Manager.instance.player.GetComponent<IHaveHP>().Heal(Manager.instance.player.maxHealth);
            }
            else
            {
                Manager.instance.player.GetComponent<IHaveHP>().Heal(healAmount);
            }
            Manager.instance.PlaySound(transform.position, use);
        }
        else
        {
            Manager.instance.PlaySound(transform.position, deny);
        }
    }
    IEnumerator LightBulb()
    {
        canUse = false;
        bulb.color = closedColor;
        yield return new WaitForSeconds(timeOfLight);
        canUse = true;
        bulb.color = openColor;
    }
}