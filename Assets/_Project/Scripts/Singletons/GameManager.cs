using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonPattern<GameManager>
{
    public Camera _mainCamera;

    public GameObject _player;

    private void Awake()
    {
        base.Init(this, false);
    }
}
