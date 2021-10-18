using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;


    protected void Awake()
    {
        MakeSingleton();
    }

    protected void Start()
    {
    }

    protected void Update()
    {
    }

    private void MakeSingleton()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}