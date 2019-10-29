using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardScript;

    private int level = 3;
    //https://learn.unity.com/tutorial/level-generation?projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f7 4:00 left off
    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
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
