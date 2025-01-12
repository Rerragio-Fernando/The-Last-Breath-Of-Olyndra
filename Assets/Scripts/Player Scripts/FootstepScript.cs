using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepClipsWalk;
    [SerializeField] private AudioClip[] _footstepClipsRun;
    private AudioSource _source;

    private void Start() {
        _source = GetComponent<AudioSource>();
    }

    public void StepRun(){
        _source.PlayOneShot(_footstepClipsRun[Random.Range(0, _footstepClipsRun.Length)]);
    }
    public void StepWalk(){
        _source.PlayOneShot(_footstepClipsWalk[Random.Range(0, _footstepClipsWalk.Length)]);
    }
}