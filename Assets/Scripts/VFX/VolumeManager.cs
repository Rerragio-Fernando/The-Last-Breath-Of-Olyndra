using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeManager : MonoBehaviour
{
    [Header("Boost Volume")]
    [SerializeField] private Volume _boostVolume;
    [SerializeField] private float _boostVolumeSmoothTime;

    [Header("Attack Hit Volume")]
    [SerializeField] private Volume _attackVolume;
    [SerializeField] private float _attackVolumeSmoothTime;

    private bool _boostTrigger = false;
    private bool _attackHitTrigger = false;

    private void Start() {
        PlayerEventSystem.OnCharacterBoostInEvent += BoostVolumeOn;
        PlayerEventSystem.OnSuccessfulHitEvent += AttackVolumeOn;
    }

    private void Update() {
        if(_boostTrigger)
            _boostVolume.weight = Mathf.Lerp(_boostVolume.weight, 1f, _boostVolumeSmoothTime * Time.deltaTime);
        else
            _boostVolume.weight = Mathf.Lerp(_boostVolume.weight, 0f, _boostVolumeSmoothTime * Time.deltaTime);

        if(_attackHitTrigger)
            _attackVolume.weight = Mathf.Lerp(_attackVolume.weight, 1f, _attackVolumeSmoothTime * Time.deltaTime);
        else
            _attackVolume.weight = Mathf.Lerp(_attackVolume.weight, 0f, _attackVolumeSmoothTime * Time.deltaTime);
    }

    private void AttackVolumeOn(){
        StartCoroutine(AttackVolume());
    }
    
    IEnumerator AttackVolume(){
        _attackHitTrigger = true;

        yield return new WaitForSeconds(.25f);

        _attackHitTrigger = false;
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