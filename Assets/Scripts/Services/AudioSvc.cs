﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoBehaviour
{
    #region 单例模式
    private static AudioSvc _instance = null;

    public static AudioSvc Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<AudioSvc>();
            }
            return _instance;
        }
    }
    #endregion

    public AudioSource bgMusicAudio;
    public AudioSource effectSoundAudio;

    public bool isActiveBgMusic;
    public bool isActiveEffectSound;

    public void InitSvc()
    {
        isActiveBgMusic = true;
        isActiveEffectSound = true;
        Debug.Log("Init AudioSvc Done");
    }

    //停止背景音乐
    public void StopBgMusic()
    {
        if(bgMusicAudio != null)
        {
            bgMusicAudio.Stop();
        }
    }

    //播放背景音乐
    public void PlayBgMusic(string clipPath,bool isLoop = true)
    {
        AudioClip clip = ResSvc.Instance.LoadAudioClip(clipPath, true);
        if(bgMusicAudio.clip == null || bgMusicAudio.clip.name != clip.name)
        {
            bgMusicAudio.clip = clip;
            bgMusicAudio.loop = isLoop;
            bgMusicAudio.Play();
        }
    }

    //设置背景音乐音量
    public void SetBgMusicVolumn(float volumn)
    {
        bgMusicAudio.volume = volumn;
    }

    //激活背景音乐
    public void ActiveBgMusic()
    {
        isActiveBgMusic = true;
        if (bgMusicAudio != null)
        {
            bgMusicAudio.mute = false;
        }
    }

    //禁用背景音乐
    public void InActiveBgMusic()
    {
        isActiveBgMusic = false;
        if (bgMusicAudio != null)
        {
            bgMusicAudio.mute = true;
        }
    }

    //播放音效
    public void PlayEffectSound(string clipPath)
    {
        AudioClip clip = ResSvc.Instance.LoadAudioClip(clipPath, true);
        effectSoundAudio.PlayOneShot(clip);
    }

    //播放音效音量
    public void SetEffectSoundVolumn(float volumn)
    {
        effectSoundAudio.volume = volumn;
    }

    //激活游戏音效
    public void ActiveEffectSound()
    {
        isActiveEffectSound = true;
        if(effectSoundAudio != null)
        {
            effectSoundAudio.mute = false;
        }
    }

    //禁用游戏音效
    public void InActiveEffectSound()
    {
        isActiveEffectSound = false;
        if (effectSoundAudio != null)
        {
            effectSoundAudio.mute = true;
        }
    }

}
