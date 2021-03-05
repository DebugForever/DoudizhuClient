using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource audioEffect;
    private AudioSource audioBackGround;

    private AudioClip bg;
    private AudioClip sendCard;


    private void Awake()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        if (audios.Length < 2)
            throw new MissingComponentException("AudioManager need 2 AudioSources");
        audioEffect = audios[0];
        audioBackGround = audios[1];
    }

    protected override void Init()
    {
        base.Init();
        sendCard = ResourceManager.GetSendCardAudio();
        bg = ResourceManager.GetBGAudio();
        audioBackGround.clip = bg;
        audioBackGround.loop = true;
        audioBackGround.Play();
    }

    public void PlaySendCard()
    {
        audioEffect.PlayOneShot(sendCard);
    }
}
