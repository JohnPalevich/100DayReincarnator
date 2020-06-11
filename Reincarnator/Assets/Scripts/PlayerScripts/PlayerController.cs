using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public static PlayerController player = null;
    public Camera camera;
    public float speed;
    public float knockback;
    public GameObject boomerangPrefab;
    public GameObject swordPrefab;

    private Rigidbody2D rb2d;
    private bool thrown;
    private bool swung;
    private int coins = 0;
    private float health = 30f;
    private float maxHealth = 30f;
    private GameObject boomerang;
    private GameObject sword;
    private int enemiesGE = 0;
    private GameObject currWeapon;
    private float boomerangDamage = 1f;
    private float swordDamage = 1f;

    void Start(){
        if(player == null)
        {
            player = this;
        }
        else if(player != this)
        {
            Destroy(this);
        }
        rb2d = GetComponent<Rigidbody2D>();

        setUpSelf();

        GameManager.instance.SetCoinText(coins);            //Turns on Coin Text on top Left
        GameManager.instance.SetHealth(health / maxHealth);

        rb2d.freezeRotation = true;                         //Ensures It doesn't rotate after collisions

        thrown = false;
        swung = false;
        updateBoomerang();
        sword = swordPrefab;
        currWeapon = boomerang;
    }

    private void setUpSelf()
    {
        Dictionary<string, string> info = GameManager.instance.retrievePlayerInfo();
        coins = int.Parse(info["coins"]);
        health = float.Parse(info["health"]);
        maxHealth = float.Parse(info["maxHealth"]);
        enemiesGE = int.Parse(info["EnemiesGE"]);
    }

    void FixedUpdate(){
        float moveHorz = Input.GetAxis("Horizontal");               //Gets "A" or "D" inputs
        float moveVert = Input.GetAxis("Vertical");                 //Gets "W" or "S" inputs
        Vector2 movement = new Vector2(moveHorz, moveVert);
        
        movement.Normalize();
        rb2d.AddForce(movement * speed);                            //Pushes the play object
        if (Input.GetMouseButtonDown(0))                            //Checks to see if mouse is being clicked and if it hasn't been thrown
        {
            if(currWeapon.Equals(boomerangPrefab) && !thrown)
            {
                throwBoomerang();
            }
            else if(currWeapon.Equals(swordPrefab) && !swung)
            {
                swordAttack();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currWeapon = boomerangPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currWeapon = swordPrefab;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Coin")){
            Destroy(other.gameObject);
            coins++;
            GameManager.instance.SetCoinText(coins);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;
            GameManager.instance.SetHealth(health / maxHealth);
            rb2d.AddForce(collision.relativeVelocity * knockback );
        }
        else if (collision.gameObject.CompareTag("Bullets"))
        {
            health -= 1;
            GameManager.instance.SetHealth(health / maxHealth);
            Vector2 knock = collision.relativeVelocity.normalized;
            rb2d.AddForce(knock);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boomerang"))
        {
            Object.Destroy(boomerang);
            updateBoomerang();
            thrown = false;
        }
        else if (collision.gameObject.CompareTag("Exit"))
        {
            GameManager.instance.wrapUpLevel(keepImportantInformation());
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            if (collision.gameObject.name.Contains("sPowerup"))
            {
                swordDamage++;
            }
            else
            {
                boomerangDamage++;
            }
            Destroy(collision.gameObject);
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void throwBoomerang()
    {
        Vector3 mousePos = Input.mousePosition;             
        mousePos = camera.ScreenToWorldPoint(mousePos);         //Converts mouse position on screen to position in-game. 
        Vector3 difference = new Vector3(transform.position.x - mousePos.x, transform.position.y - mousePos.y, 0) * -1;
        difference.Normalize();
        //Debug.Log("Mouse X:" + mousePos.x + "Mouse Y:" + mousePos.y);     //Print statement for debugging mouse position
        Vector3 spawnPos = transform.position + (difference * 1.3f);
        //Debug.Log("Spawn X: " + spawnPos.x + " Spawn Y: " + spawnPos.y);  //Print statement for debugging spawnPosition of the boomerang

        //Creates the object and organizes the object in the heirarchy
        boomerang = Instantiate(boomerangPrefab, spawnPos, Quaternion.identity);
        boomerang.transform.parent = transform;
        
        //Calculations for the direction in which the boomerang will travel in.
        Rigidbody2D boomRB2D = boomerang.GetComponent<Rigidbody2D>();
        float dirX = (transform.position.x - boomerang.transform.position.x) * -1;
        float dirY = (transform.position.y - boomerang.transform.position.y) * -1;
        Vector2 movement = new Vector2(dirX, dirY);
        boomRB2D.AddForce(movement * 275);
        thrown = true;
    }

    public void updateBoomerang()
    {
        boomerang = boomerangPrefab;
    }

    private void swordAttack()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = camera.ScreenToWorldPoint(mousePos);         //Converts mouse position on screen to position in-game. 
        Vector3 difference = new Vector3(transform.position.x - mousePos.x, transform.position.y - mousePos.y, 0) * -1;
        difference.Normalize();
        Vector3 spawnPos = transform.position + (difference * 1.3f);
        float xDiff = spawnPos.x - transform.position.x;
        float yDiff = spawnPos.y - transform.position.y;
        if(System.Math.Abs(yDiff) > System.Math.Abs(xDiff))
        {
            if (yDiff > 0)
            {
                spawnPos = new Vector3(transform.position.x, transform.position.y + 1f);
                sword = Instantiate(swordPrefab, spawnPos, Quaternion.identity);
                sword.transform.Rotate(new Vector3(0, 0, 90));
            }
            else
            {
                spawnPos = new Vector3(transform.position.x, transform.position.y - 1f);
                sword = Instantiate(swordPrefab, spawnPos, Quaternion.identity);
                Vector3 newScale = sword.transform.localScale;
                newScale.x *= -1;
                sword.transform.localScale = newScale;
                sword.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
        else
        {
            if (xDiff > 0)
            {
                spawnPos = new Vector3(transform.position.x + 1f, transform.position.y);
                sword = Instantiate(swordPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                spawnPos = new Vector3(transform.position.x - 1f, transform.position.y);
                sword = Instantiate(swordPrefab, spawnPos, Quaternion.identity);
                Vector3 newScale = sword.transform.localScale;
                newScale.x *= -1;
                sword.transform.localScale = newScale;
            }
        }
        
        sword.transform.parent = transform;
        swung = true;
    }

    public void finSwing()
    {
        swung = false;
    }

    public float bDamage()
    {
        return boomerangDamage;
    }

    public float sDamage()
    {
        return swordDamage;
    }

    public void setSDamage(int dmg)
    {
        swordDamage = dmg;
    }

    public void setBDamage(int dmg)
    {
        boomerangDamage = dmg;
    }
    public Dictionary<string, string> keepImportantInformation()
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info.Add("health", health.ToString());
        info.Add("maxHealth", maxHealth.ToString());
        info.Add("coins", coins.ToString());
        info.Add("EnemiesGE", enemiesGE.ToString());
        return info;
    }
}
