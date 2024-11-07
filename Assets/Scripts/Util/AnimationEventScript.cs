using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventScript : MonoBehaviour
{
    //Event Method
    public void AnimationJump(){
        GameEventSystem._current.AnimationJump();
    }
    public void Test(){
        Debug.Log($"Hukanawa");
    }
}