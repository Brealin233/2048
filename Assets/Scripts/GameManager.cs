using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public InputManager inputManager;

    private void Awake()
    {
        Instance = this;
        
        inputManager = new InputManager();
        inputManager.Enable();
    }
}