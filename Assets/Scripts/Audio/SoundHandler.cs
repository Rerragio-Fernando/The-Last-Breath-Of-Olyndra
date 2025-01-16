using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public Sound[] soundFx;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeAudio();
    }

    public void InitializeAudio(){
        foreach(Sound s in soundFx){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.priority = s.priority;
            s.source.spatialBlend = s.spatialBlend;
            s.source.maxDistance = s.spatialSoundMaxDistance;
        }
    }

    public void PlaySound(string name){
        Sound s = Array.Find(soundFx, sound => sound.name == name);
        s.source.Play();
    }
    public void PlaySound(string name, float p){
        Sound s = Array.Find(soundFx, sound => sound.name == name);
        s.source.pitch = p;
        s.source.Play();
    }
    public void PlayRandomSound(){
        if(soundFx.Length > 0){
            Sound s = soundFx[UnityEngine.Random.Range(0, soundFx.Length - 1)];
            s.source.Play();
        }
    }
    public void PlaySound(int x){
        var s = soundFx[x];
        s.source.Play();
    }
    public void PlaySound(int x, float p){
        var s = soundFx[x];
        s.source.pitch = p;
        s.source.Play();
    }
    public void PlaySoundWithRandomPitch(string name){
        Sound s = Array.Find(soundFx, sound => sound.name == name);
        s.source.pitch = UnityEngine.Random.Range(1f, 2f);
        s.source.Play();
    }
    public void StopSound(int x){
        var s = soundFx[x];
        s.source.Stop();
    }
}
