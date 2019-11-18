using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController player = null;
    public float speed;
    public float knockback;
    public GameObject prefab;

    private Rigidbody2D rb2d;
    private bool thrown;
    private int coins;
    private int health;
    private GameObject boomerang;
    
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
        coins = 0;
        health = 30;
        GameManager.instance.SetCoinText(coins);
        GameManager.instance.SetLifeText(health);
        rb2d.freezeRotation = true;
        thrown = false;
        updateBoomerang();
    }
    void FixedUpdate(){
        float moveHorz = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorz, moveVert);
        
        movement.Normalize();
        rb2d.AddForce(movement * speed);
        if (Input.GetKeyDown(KeyCode.Space) && !thrown)
        {
            throwBoomerang();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);
            coins++;
            GameManager.instance.SetCoinText(coins);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;
            GameManager.instance.SetLifeText(health);
            rb2d.AddForce(collision.relativeVelocity * knockback );
        }
        else if (collision.gameObject.CompareTag("Bullets"))
        {
            health -= 1;
            GameManager.instance.SetLifeText(health);
            Vector2 knock = collision.relativeVelocity.normalized;
            rb2d.AddForce(knock);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boomerang"))
        {
            Destroy(boomerang);
            thrown = false;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void throwBoomerang()
    {
        Vector3 mousePosition = Input.mousePosition;
        float slope = (this.transform.position.y - mousePosition.y) / (this.transform.position.x - mousePosition.x);
        float xOffset = 0.1f;
        if(this.transform.position.x - mousePosition.x < 0)
        {
            xOffset *= -1;
        }
        float yOffset = ((this.transform.position.y) + slope * (this.transform.position.x + xOffset));
        boomerang = Instantiate(prefab, new Vector3(this.transform.position.x + xOffset, yOffset, 0), Quaternion.identity);
        thrown = true;
    }

    public void updateBoomerang()
    {
        boomerang = prefab;
    }
}
