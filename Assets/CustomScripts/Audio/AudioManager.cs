using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Array storing all the sounds used in the game
    /// </summary>
    public Sound[] sounds;

    /// <summary>
    /// Adding audio sources to all our sounds
    /// </summary>
    private void Awake ()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Script to play any sound
    /// </summary>
    /// <param name="name">Name of sound to play</param>
    public void Play(string name)
    {
        // Searching in sounds such that the name of the sound is equal to the requested name
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        if (s == null) // If nothing is found, print an error message and don't attempt to play the sound
        {
            Debug.Log("Error playing the sound");
            return;
        }
        s.source.Play();
    }
}
