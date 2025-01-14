using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 2f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend;

    [Range(0, 256)]
    public int priority;

    public float spatialSoundMaxDistance = 500f;

    [HideInInspector]
    public AudioSource source;
}
