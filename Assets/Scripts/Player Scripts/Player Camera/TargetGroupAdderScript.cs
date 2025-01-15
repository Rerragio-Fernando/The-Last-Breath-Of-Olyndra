using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class TargetGroupAdderScript : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;

    [Header("Player Group Settings")]
    [SerializeField] private float _playerWeight = 1f;
    [SerializeField] private float _playerRadius = 1f;

    [Header("Enemy Group Settings")]
    [SerializeField] private float _enemyWeight = 1f;
    [SerializeField] private float _enemyRadius = 1f;

    private Transform _enemyTrans;
    private Transform _playerTrans;

    private void Start() {
        _enemyTrans = GameObject.FindWithTag("Enemy").transform;
        _playerTrans = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        AddObjectsToGroup();
    }

    void AddObjectsToGroup(){
        if(_enemyTrans == null){
            _enemyTrans = GameObject.FindWithTag("Enemy").transform;
            _targetGroup.AddMember(_enemyTrans, _enemyWeight, _enemyRadius);
        }

        if(_playerTrans == null){
            _playerTrans = GameObject.FindWithTag("Player").transform;
            _targetGroup.AddMember(_playerTrans, _playerWeight, _playerRadius);
        }
    }
}