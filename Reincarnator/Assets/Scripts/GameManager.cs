using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    private BoardManager boardScript;
    public GameObject player;
    public CameraController camera;

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
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetUpScene(level);
        player = boardScript.player;
        CameraController.camera.player = player;
        CameraController.camera.setOffset();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
