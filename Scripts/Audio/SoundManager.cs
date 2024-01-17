﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioPlayer;

    [SerializeField]
    private float genericVolumeMulitplier = 1;

    [Header("UI Sounds")]
    public AudioClip uiClickedSound;
    public AudioClip uiCloseSound;
    public AudioClip uiPauseSound;
    public AudioClip uiPlaySound;
    public AudioClip uiFailSound;
    public AudioClip uiTapToPlaySound;
    public AudioClip uiTappedSound;
    public AudioClip sparkSound;
    public AudioClip cryptoCoinSound;
    public AudioClip tiktik;


    public AudioClip[] boneCrushSound;

    public static SoundManager Instance;

    void Awake ()
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


    //public void PlayHeadPopSounds(int count)
    //{
    //    StartCoroutine(PlayHeadPopSound(Mathf.Clamp(count, 0, headPopSound.Length)));
    //}

    //public IEnumerator PlayHeadPopSound(int headPopCount, float intensity = 2.0f)
    //{
    //    List<AudioClip> headPopSoundsList = new List<AudioClip>();

    //    for (int n = 0; n < headPopSound.Length; n++)
    //    {
    //        headPopSoundsList.Add(headPopSound[n]);
    //    }

    //    yield return new WaitForSeconds(1f);

    //    for (int i = 0; i < headPopCount; i++)
    //    {
    //        int rnadomIndex = Random.Range(0, headPopSoundsList.Count);
    //        AudioClip clip = headPopSoundsList[rnadomIndex];

    //        if (audioPlayer)
    //        {
    //            audioPlayer.PlayOneShot(clip, GameManager.Instance.soundValue * intensity);
    //        }

    //        headPopSoundsList.Remove(clip);
    //        PlayZombvieDeathSound(0.8f);
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

    public void PlayRewardPanelSound()
    {
        
    }

    public void PlaySparkSound(float intensity = 1.0f)
    {
        if (audioPlayer)
        {
            audioPlayer.PlayOneShot(sparkSound, GameManager.Instance.soundValue * intensity);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayUIClickedSound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiClickedSound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayUICloseSound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiCloseSound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayPauseSound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiPauseSound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayPlaySound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiPlaySound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayFailSound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiFailSound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayTaptoplaySound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiTapToPlaySound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayTappedSound()
    {
        if (audioPlayer)
        {
            if (uiClickedSound)
            {
                audioPlayer.PlayOneShot(uiTappedSound, GameManager.Instance.soundValue * genericVolumeMulitplier);
            }
            else
                Utility.ErrorLog("Sound of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }

    public void PlayTikTik(float intensity = 1.0f)
    {
        if (GameManager.Instance.soundValue < 0)
        {
            return;
        }
        audioPlayer.PlayOneShot(tiktik, intensity);
    }


    public void PlayCryptoCoin(float intensity = 1.0f)
    {
        if (audioPlayer)
        {
            audioPlayer.PlayOneShot(cryptoCoinSound, GameManager.Instance.soundValue * intensity);
        }
        else
            Utility.ErrorLog("Audio Source of " + this.gameObject.name + " in SoundManager.cs is not assigned", 1);
    }
}