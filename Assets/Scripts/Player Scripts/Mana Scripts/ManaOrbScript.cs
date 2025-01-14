using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrbScript : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private int _manaGain;

    private Rigidbody _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody>();

        _rb.AddForce(new Vector3(Random.Range(0f, 1f), 1f, Random.Range(0f, 1f)) * _force, ForceMode.Impulse);
    }
}