using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TriggerProfile", menuName = "TriggerProfile")]
public class TriggerProfile : ScriptableObject
{
    [Range(0, 1)]
    public int _triggerProfile;

    [Range(0, 1)]
    public float _force;

    [Range(0, 1)]
    public float _startPosition;

    [Range(0, 1)]
    public float _endPosition;

    public TriggerProfile(){
        this._triggerProfile = 0;
        this._force = 0f;
        this._startPosition = 0f;
        this._endPosition = 0f;
    }
}