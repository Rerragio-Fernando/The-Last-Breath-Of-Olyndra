using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacterEventSystem : Singleton<GameCharacterEventSystem>
{
    public static event Action OnSpawnInEvent;
    public static event Action OnDeathEvent;

    private void Start() {
        TriggerSpawnEvent();
    }

    public static void TriggerSpawnEvent(){
        OnSpawnInEvent?.Invoke();
    }

    public static void TriggerDeathEvent(){
        OnDeathEvent?.Invoke();
    }
}