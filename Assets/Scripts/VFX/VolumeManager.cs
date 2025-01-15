using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Volume _boostVolume;
    [SerializeField] private float _boostVolumeSmoothTime;

    private bool _boostTrigger = false;

    private void Start() {
        PlayerEventSystem.OnCharacterBoostInEvent += BoostVolumeOn;
    }

    private void Update() {
        if(_boostTrigger)
            _boostVolume.weight = Mathf.Lerp(_boostVolume.weight, 1f, _boostVolumeSmoothTime * Time.deltaTime);
        else
            _boostVolume.weight = Mathf.Lerp(_boostVolume.weight, 0f, _boostVolumeSmoothTime * Time.deltaTime);
    }

    private void BoostVolumeOn(){
        StartCoroutine(BoostVolume());
    }
    
    IEnumerator BoostVolume(){
        _boostTrigger = true;

        yield return new WaitForSeconds(.25f);

        _boostTrigger = false;
    }
}