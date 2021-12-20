using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicStarter : MonoBehaviour
{
    public static BackgroundMusicStarter instance;
    public AudioClip backMusic;
    AudioSource source;
    [SerializeField] bool playOnAwake = true;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
        if (playOnAwake) Play();
    }
    public void Play()
    {
        source.clip = backMusic;
        source.volume = 0;
        source.Play();
        Manager.instance.RaiseAudio(source, 0.1f);
    }
    public void Stop()
    {
        Manager.instance.FadeAudio(source, 0.1f);
    }
    public void StopImmideate()
    {
        source.Stop();
    }
    public void ChangeClip(AudioClip newClip)
    {
        backMusic = newClip;
        Play();
    }
}
