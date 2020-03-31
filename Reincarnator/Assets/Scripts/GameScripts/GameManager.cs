using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    public GameObject player;
    public Canvas canvas;

    private BoardManager boardScript;
    private Text coinText;
    private Transform hpBar;
    private int level = 1;
    private float health;
    private float maxHealth;
    private int coins;
    
    //Initiates the boardManager as well as the text on the screen.
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
        setUpLevel();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        hpBar = GameObject.Find("PlayerHPBar").transform;
    }

    //Initiates the Level
    void InitGame()
    {
        boardScript.SetUpScene(level);
    }

    public void SetCoinText(int coins)
    {
        coinText.text = "Coins: " + coins.ToString();
    }

    public void SetHealth(float hpLeft)
    {
        Transform bar = hpBar.Find("Bar");
        bar.localScale = new Vector3(hpLeft, 1f, 1f); ;
    }


    public void wrapUpLevel(Dictionary<string,string> info)
    {
        
        string serializedData =
        "health, " + info["health"] + "\n" +
        "maxHealth, " + info["maxHealth"] + "\n"+
        "coins, " + info["coins"] + "\n" +
        "day, " + level.ToString() + "\n";

        // Write to disk
        StreamWriter writer = new StreamWriter("Assets/SaveData/saveData.txt", true);
        writer.Write(serializedData);
    }

    public void setUpLevel()
    {
        StreamReader reader = new StreamReader("Assets/SaveData/saveData.txt");
        health = int.Parse(reader.ReadLine().Split(',')[1]);
        maxHealth = int.Parse(reader.ReadLine().Split(',')[1]);
        coins = int.Parse(reader.ReadLine().Split(',')[1]);
        level = int.Parse(reader.ReadLine().Split(',')[1]);
        boardScript.SetUpScene(level);
    }

    public Dictionary<string, string> retrievePlayerInfo()
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info.Add("health", health.ToString());
        info.Add("maxHealth", maxHealth.ToString());
        info.Add("coins", coins.ToString());
        return info;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
