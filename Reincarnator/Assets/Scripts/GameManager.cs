﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    private BoardManager boardScript;
    public GameObject player;

    private int level = 3;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        boardScript.player = player;
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetUpScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
