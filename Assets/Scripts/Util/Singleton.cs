using UnityEngine.SceneManagement;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning($"{typeof(T).Name} instance is null. Ensure it is initialized.");
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            SceneManager.sceneLoaded -= HandleSceneLoaded; // Cleanup event subscription
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_instance == this)
        {
            OnSceneLoaded(scene, mode);
        }
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }
}
