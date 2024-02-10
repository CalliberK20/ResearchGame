using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string audioName;
    [Space]
    public bool addComponent = false;
    [Space]
    [Range(0f, 1f)]
    public float volume = 1;
    public AudioClip clip;
    public AudioMixerGroup mixer;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound bgSound;
    public Sound[] soundList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        foreach (var sound in soundList)
        {
            if (!sound.addComponent)
                continue;
            sound.source = CreateComponentSound(sound);
        }
        bgSound.source = CreateComponentSound(bgSound);
        bgSound.source.Play();
    }

    private AudioSource CreateComponentSound(Sound newSound)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.volume = newSound.volume;
        audio.clip = newSound.clip;
        audio.outputAudioMixerGroup = newSound.mixer;
        audio.loop = newSound.loop;
        return audio;
    }

    public Sound GetAudio(string audioName)
    {
        Sound sound = Array.Find(soundList, s  => s.audioName == audioName);
        if (sound == null)
            return null;
        return sound;
    }

    public void PlayAudio(string audioName)
    {
        Sound sound = Array.Find(soundList, s => s.audioName == audioName);
        if (sound == null)
            return;

        if (!sound.addComponent)
        {
            Debug.Log("Sounds is not created as a component in the Manager");
            return;
        }

        sound.source.Play();
    }
}
