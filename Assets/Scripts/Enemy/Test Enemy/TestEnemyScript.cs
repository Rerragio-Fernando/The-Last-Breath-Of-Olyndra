using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemyScript : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Transform _player;

    private void Start() {
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        _agent.SetDestination(_player.position);
    }
}