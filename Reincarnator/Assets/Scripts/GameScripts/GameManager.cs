using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    private BoardManager boardScript;
    public GameObject player;
    public Canvas canvas;
    private Text healthText;
    private Text coinText;
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
        healthText = GameObject.Find("LifeText").GetComponent<Text>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();

    }

    void InitGame()
    {
        boardScript.SetUpScene(level);
    }

    public void SetCoinText(int coins)
    {
        coinText.text = "Coins: " + coins.ToString();
    }

    public void SetLifeText(int health)
    {
        healthText.text = "Life: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
