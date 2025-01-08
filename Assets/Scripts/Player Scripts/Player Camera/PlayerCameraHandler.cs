using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerCameraHandler : MonoBehaviour
{
    public static PlayerCameraHandler _current;
    [SerializeField] private CinemachineBrain _cineBrain;
    [SerializeField] private GameObject _cinemachineCamera;
    
    [Header("Normal Camera")]
    [SerializeField] private Transform _targetNormal;
    [SerializeField] private float _mainFov;
    [SerializeField] private float _mainToAimBlendTime;
    [SerializeField] private Vector3 _NormalTargetRotationOffset;
    [SerializeField] private Vector3 _NormalTargetPositionOffset;

    [Header("Aim Camera")]
    [SerializeField] private Transform _targetAim;
    [SerializeField] private float _aimFov;
    [SerializeField] private float _aimToMainBlendTime;
    [SerializeField] private Vector3 _AimTargetRotationOffset;
    [SerializeField] private Vector3 _AimTargetPositionOffset;

    private CinemachineCamera _cineCam;
    private CinemachineRotationComposer _cineCamRot;
    private CinemachineOrbitalFollow _cineCamFollow;

    private void Awake() {
        if(_current == null)
            _current = this;
        else
            Destroy(this);
    }

    private void Start() {
        _cineCam = _cinemachineCamera.GetComponent<CinemachineCamera>();
        _cineCamRot = _cinemachineCamera.GetComponent<CinemachineRotationComposer>();
        _cineCamFollow = _cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
    }

    public void ActivateMainCam(){
        _cineBrain.DefaultBlend.Time = _aimToMainBlendTime;
        SetCineCamPresets(_targetNormal, _NormalTargetRotationOffset, _NormalTargetPositionOffset, _mainFov, _aimToMainBlendTime);
    }

    public void ActivateAimCam(){
        _cineBrain.DefaultBlend.Time = _mainToAimBlendTime;
        SetCineCamPresets(_targetAim, _AimTargetRotationOffset, _AimTargetPositionOffset, _aimFov, _mainToAimBlendTime);
    }

    void SetCineCamPresets(Transform _target, Vector3 rot, Vector3 pos, float fov, float blendTime){
        _cineCam.Target.TrackingTarget = _target;
        _cineCam.Lens.FieldOfView = Mathf.Lerp(_cineCam.Lens.FieldOfView, fov, blendTime * Time.deltaTime);
        _cineCamRot.TargetOffset = Vector3.Lerp(_cineCamRot.TargetOffset, rot, blendTime * Time.deltaTime);
        _cineCamFollow.TargetOffset = Vector3.Lerp(_cineCamFollow.TargetOffset, pos, blendTime * Time.deltaTime);
    }
}