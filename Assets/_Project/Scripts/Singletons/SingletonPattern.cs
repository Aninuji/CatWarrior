using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPattern<T> : MonoBehaviour
{
    private static T _Instance;

    public static T Instance
    {
        get { return _Instance; }
    }

    protected void Init(T type, bool isPersistent)
    {
        if (Instance == null)
        {
            _Instance = type;
            if (isPersistent)
                DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
