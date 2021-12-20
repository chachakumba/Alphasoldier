using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject virtCam;
    [SerializeField] BossTurret boss;
    [SerializeField] Door[] doors;
    bool activated = false;
    [SerializeField] float waitTillBossAwakeTime = 1;
    [SerializeField] float waitTime = 1;
    [SerializeField] float returnTime = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            virtCam.gameObject.SetActive(true);
            foreach (Door door in doors)
            {
                door.closed = true;
            }
            boss.OnDeath += BossDefeat;
            activated = true;
            StartCoroutine(CutScene());
        }
    }
    IEnumerator CutScene()
    {
        BackgroundMusicStarter.instance.Stop();
        Manager.instance.DisablePlayerControls();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTillBossAwakeTime);
        boss.enabled = true;
        yield return new WaitForSecondsRealtime(waitTime);
        virtCam.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(returnTime);
        Manager.instance.EnablePlayerControls();
        boss.ActivateBoss();
        Time.timeScale = 1;
    }
    void BossDefeat(object sender, System.EventArgs arg)
    {
        foreach (Door door in doors)
        {
            door.closed = false;
        }
    }
}
