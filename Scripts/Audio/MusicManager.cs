using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioPlayer;
    [SerializeField]
    private float genericVolumeMulitplier = 1f;

    [Header("Musics Backgrounds")]
    public AudioClip MainMenuMusic;
    public AudioClip[] IngameMusic;

    public static MusicManager Instance;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        if (!audioPlayer)
        {
            if (GetComponent<AudioSource>())
            {
                audioPlayer = GetComponent<AudioSource>();
            }
        }
    }

    public void PlayMainMenuMusic()
    {
        if (audioPlayer)
        {
            if (MainMenuMusic)
            {
                audioPlayer.clip = MainMenuMusic;
                audioPlayer.Play();
            }
            else
                Utility.ErrorLog("Main Menu of " + this.gameObject.name + " in MusicManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in MusicManager.cs is not assigned", 1);
    }

    public void PlayIngameMusic()
    {
        if (audioPlayer)
        {
            if (MainMenuMusic)
            {
                audioPlayer.clip = IngameMusic[Random.Range(0, IngameMusic.Length)];
                audioPlayer.Play();
            }
            else
                Utility.ErrorLog("Ingame of " + this.gameObject.name + " in MusicManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in MusicManager.cs is not assigned", 1);
    }
    public void AdjustVolumeInStart()
    {
        if (GameManager.Instance.musicValue > 0.3f)
        {
            audioPlayer.volume = 0.3f;
        }
        else
        {
            audioPlayer.volume = GameManager.Instance.musicValue;
        }
        audioPlayer.pitch = 1f;
    }
    public void LerpSoundVolume(bool high)
    {
        StartCoroutine(LerpVolume(high));
    }
    public void LerpSoundSpeed(bool fast)
    {
        StartCoroutine(LerpSpeed(fast));
    }
    IEnumerator LerpVolume(bool high)
    {
        float a;
        float b;
        if(high)
        {
            a = audioPlayer.volume;
            b = 1f;
            b = Mathf.Clamp(b, a, GameManager.Instance.musicValue);
        }
        else
        {
            a = audioPlayer.volume;
            b = 0.3f;
            if (GameManager.Instance.musicValue < 0.3f)
            {
                b = Mathf.Clamp(b, GameManager.Instance.musicValue, 0.3f);
            }
        }
        float t = 1;
        float n = 0;
        for (float f = 0; f <= t; f += Time.deltaTime)
        {
            n = Mathf.Lerp(a, b, f / t);
            audioPlayer.volume = n;
            yield return null;
        }
        StopAllCoroutines();
        LerpSoundSpeed(high);
    }
    IEnumerator LerpSpeed(bool fast)
    {
        float a = audioPlayer.pitch;
        float b = 1.25f;////
        float t = 15;
        if (fast == false)
        {
            a = audioPlayer.pitch;
            b = 1f;
            t = 2f;
        }
        float n = 0;
        for (float f = 0; f <= t; f += Time.deltaTime)
        {
            n = Mathf.Lerp(a, b, f / t);
            audioPlayer.pitch = n;
            yield return null;
        }
    }
}