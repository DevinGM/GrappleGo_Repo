using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Devin G Monaghan
/// 9/17/2024
/// handles singleton
/// </summary>

public class Singleton<T> : MonoBehaviour where T : Component
{
    // private reference to this object
    private static T _instance;

    // public reference to this object
    public static T Instance
    {
        get
        {
            // if there isn't a private ref yet, make one
            if (_instance == null)
            {
                // find an object of the given type and make it the private ref
                _instance = FindFirstObjectByType<T>();

                // if there's still no ref that means there's no object in the scene, make one
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            // return private ref
            return _instance;
        }
    }

    // awake is called before start
    public virtual void Awake()
    {
        // check if private ref is empty
        if (_instance == null)
        {
            // set private ref to self
            _instance = this as T;
            // allow object to persist across scenes
            DontDestroyOnLoad(gameObject);
        }
        // if private ref ISN'T empty, that means there's already an object of this type in the scene,
        // so kill self
        else
        {
            Destroy(gameObject);
        }
    }
}