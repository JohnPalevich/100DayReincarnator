using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    public GameObject player;

    private BoardManager boardScript;
    private Text coinText;
    private Transform hpBar;
    private int level = 1;
    private float health;
    private float maxHealth;
    private int coins;
    private int enemiesGE;
    private Image fade;
    private RectTransform rt;
    private GameObject exit;
    private int numEnemiesAlive;
    
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
        rt = GameObject.Find("FadeIMG").GetComponent<RectTransform>();
        rt.sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        fade = GameObject.Find("FadeIMG").GetComponent<Image>();
        Color col = new Color();
        col.a = 0;
        col.r = fade.color.r;
        col.g = fade.color.g;
        col.b = fade.color.b;
        fade.color = col;
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
        "EnemiesGE, " + info["EnemiesGE"] + "\n" +
        "day, " + level.ToString() + "\n";

        // Write to disk
        StreamWriter writer = new StreamWriter("Assets/SaveData/saveData.txt", false);
        writer.Write(serializedData);
        writer.Close();
        fadeMe();
    }

    public void setUpLevel()
    {
        StreamReader reader = new StreamReader("Assets/SaveData/saveData.txt");
        health = int.Parse(reader.ReadLine().Split(',')[1]);
        maxHealth = int.Parse(reader.ReadLine().Split(',')[1]);
        coins = int.Parse(reader.ReadLine().Split(',')[1]);
        enemiesGE = int.Parse(reader.ReadLine().Split(',')[1]);
        level = int.Parse(reader.ReadLine().Split(',')[1]);
        reader.Close();
        numEnemiesAlive = boardScript.SetUpScene(level);
        exit = GameObject.Find("Exit(Clone)");
        exit.SetActive(false);
    }
    
    public void decreaseEnemiesAlive()
    {
        numEnemiesAlive--;
        if (numEnemiesAlive <= 0)
        {
            exit.SetActive(true);
        }
    }


    public Dictionary<string, string> retrievePlayerInfo()
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info.Add("health", health.ToString());
        info.Add("maxHealth", maxHealth.ToString());
        info.Add("coins", coins.ToString());
        info.Add("EnemiesGE", enemiesGE.ToString());
        return info;
    }

    public void fadeMe()
    {
        StartCoroutine(doFade());
    }

    public void unfadeMe()
    {
        StartCoroutine(undoFade());
    }

    IEnumerator doFade()
    {
        float alpha = fade.color.a;
        while (alpha < 1)
        {
            alpha += 1f * Time.deltaTime;
            Color col = new Color();
            col.a = alpha;
            col.r = fade.color.r;
            col.g = fade.color.g;
            col.b = fade.color.b;
            fade.color = col;
            yield return null;
        }
        boardScript.clearLevel();
        setUpLevel();
        exit = GameObject.Find("Exit(Clone)");
        exit.SetActive(false);
        unfadeMe();
        yield return null;
    }

    IEnumerator undoFade()
    {
        float alpha = fade.color.a;
        while (alpha > 0)
        {
            alpha -= 1f * Time.deltaTime;
            Color col = new Color();
            col.a = alpha;
            col.r = fade.color.r;
            col.g = fade.color.g;
            col.b = fade.color.b;
            fade.color = col;
            yield return null;
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
