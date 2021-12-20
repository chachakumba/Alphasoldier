using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
public class Manager : MonoBehaviour
{
    public static Manager instance;
    public Logic player;
    public CinemachineVirtualCamera currentCamera;
    public Camera mainCam;
    [SerializeField] LineRenderer linePrefab;
    public ParticleSystem particlePrefab;
    public LayerMask floorLayer;
    public LayerMask walkableLayer;
    public LayerMask entityLayer;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public Vector2 lastPlayerFoundPosition;
    public ContactFilter2D floorFilter;
    public ContactFilter2D walkableFilter;
    public ContactFilter2D entityFilter;
    public ContactFilter2D enemyFilter;
    public ContactFilter2D playerFilter;
    [SerializeField] GameObject soundObj;
    [SerializeField] Transform shootPool;
    [SerializeField] List<LineRenderer> shootLines;
    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }
    public LineRenderer GetShootLine()
    {
        LineRenderer givenLine = null;
        for(int i  = 0; i < shootLines.Count; i++)
        {
            if (!shootLines[i].gameObject.activeInHierarchy)
            {
                givenLine = shootLines[i];
            }
        }
        if(givenLine == null)
        {
            givenLine = Instantiate(linePrefab, shootPool);
            shootLines.Add(givenLine);
        }
        givenLine.gameObject.SetActive(true);
        return givenLine;
    }
    public void ReturnLine(LineRenderer line)
    {
        line.gameObject.SetActive(false);
    }
    public void ReturnLine(LineRenderer line, float time)
    {
        StartCoroutine(ReturnLineTimer(line, time));
    }
    IEnumerator ReturnLineTimer(LineRenderer line, float timeToReturn)
    {
        yield return new WaitForSeconds(timeToReturn);
        line.gameObject.SetActive(false);
    }
    private void Start()
    {
        if (SaveManager.save == null)
        {
            Debug.LogWarning("Loaded w null save!");
        }
        else
        {
            player.transform.position = LevelPartsDisabler.instance.playerLoadPos[SaveManager.save.palyerPosIndex].position;
            player.weapon.currentAmmo = SaveManager.save.playerAmmo;
            player.health = SaveManager.save.playerHealth;
            player.GetComponent<Player>().UpdateHealthUI();
            player.GetComponent<Player>().UpdateAmmoUI();
        }
    }
    public void ShakeSkreen(float intensity, float duration)
    {
        currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        StartCoroutine(ShakeStop(duration));
    }
    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        AudioSource source = Instantiate(soundObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroySound(source));
    }
    public void PlaySound(Vector3 pos, AudioClip clip, float timeToPlay)
    {
        StartCoroutine(PlaySoundCour(pos, clip, timeToPlay));
    }
    IEnumerator PlaySoundCour(Vector3 pos, AudioClip clip, float timeToPlay)
    {
        yield return new WaitForSeconds(timeToPlay);
        AudioSource source = Instantiate(soundObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroySound(source));
    }
    IEnumerator DestroySound(AudioSource source)
    {
        yield return new WaitUntil(() => source == null || !source.isPlaying);

        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }
    IEnumerator ShakeStop(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
    public void RaiseAudio(AudioSource aud, float intensity)
    {
        StartCoroutine(RaiseAudioCour(aud, intensity));
    }
    IEnumerator RaiseAudioCour(AudioSource aud, float intensity)
    {
        while (aud.volume < 1)
        {
            aud.volume += intensity;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public  void FadeAudio(AudioSource aud, float intensity)
    {
        StartCoroutine(FadeAudioCour(aud, intensity));
    }
    IEnumerator FadeAudioCour(AudioSource aud, float intensity)
    {
        while(aud.volume > 0)
        {
            aud.volume -= intensity;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DisablePlayerControls()
    {
        if (player != null)
            player.GetComponent<Player>().controls.Disable();
    }
    public void DisablePlayerControls(float time)
    {
        StartCoroutine(DisablePlayerControlsCoroutine(time));
    }
    IEnumerator DisablePlayerControlsCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        player.GetComponent<Player>().controls.Disable();
    }
    public void EnablePlayerControls()
    {
        player.GetComponent<Player>().controls.Enable();
    }
    public void EnablePlayerControls(float time)
    {
        StartCoroutine(EnablePlayerControlsCoroutine(time));
    }
    IEnumerator EnablePlayerControlsCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        player.GetComponent<Player>().controls.Enable();
    }
    public void PlayerDeath()
    {
        Debug.Log("PlayerIsDead");
        CanvasManager.instance.OpenDeath();
    }
}

[System.Serializable]
public class Saying
{
    [TextArea]
    public string text;
    public float delay = 2;
    public float speed = 1;
}