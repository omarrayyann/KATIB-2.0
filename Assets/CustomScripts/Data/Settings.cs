using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float SFXVolume = 0.5f;
    public float musicVolume = 0.5f;
    public float brightness = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        for (int i = 0; i < audioManager.sounds.Length; i++)
        {
            if (audioManager.sounds[i].tag.Equals("SFX"))
                audioManager.sounds[i].source.volume = audioManager.sounds[i].volume * SFXVolume;
            else if (audioManager.sounds[i].tag.Equals("Music"))
                audioManager.sounds[i].source.volume = audioManager.sounds[i].volume * musicVolume;
        }
    }
}
