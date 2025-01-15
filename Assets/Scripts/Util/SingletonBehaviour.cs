using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogWarning($"{typeof(T).Name} is not present in the scene. Ensure an instance is added.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); // Optional: if the singleton should persist across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }
}