using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Claaa that handles storing sound information
public class Sound
{
    public string tag;

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
}
