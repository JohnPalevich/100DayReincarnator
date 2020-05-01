using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController player = null;
    public Camera camera;
    public float speed;
    public float knockback;
    public GameObject prefab;

    private Rigidbody2D rb2d;
    private bool thrown;
    private int coins = 0;
    private float health = 30f;
    private float maxHealth = 30f;
    private GameObject boomerang;
    private 
    
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
        updateBoomerang();

    }

    private void setUpSelf()
    {
        Dictionary<string, string> info = GameManager.instance.retrievePlayerInfo();
        coins = int.Parse(info["coins"]);
        health = float.Parse(info["health"]);
        maxHealth = float.Parse(info["maxHealth"]);
    }

    void FixedUpdate(){
        float moveHorz = Input.GetAxis("Horizontal");               //Gets "A" or "D" inputs
        float moveVert = Input.GetAxis("Vertical");                 //Gets "W" or "S" inputs
        Vector2 movement = new Vector2(moveHorz, moveVert);
        
        movement.Normalize();
        rb2d.AddForce(movement * speed);                            //Pushes the play object
        if (Input.GetMouseButtonDown(0) && !thrown)                 //Checks to see if mouse is being clicked and if it hasn't been thrown
        {
            throwBoomerang();
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
        boomerang = Instantiate(prefab, spawnPos, Quaternion.identity);
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
        boomerang = prefab;
    }

    public Dictionary<string, string> keepImportantInformation()
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info.Add("health", health.ToString());
        info.Add("maxHealth", maxHealth.ToString());
        info.Add("coins", coins.ToString());
        return info;
    }
}
