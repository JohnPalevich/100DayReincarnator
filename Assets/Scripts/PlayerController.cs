using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public float knockback;
    public Text coinText;
    public Text lifeText;

    private Rigidbody2D rb2d;
    private int coins;
    private int health;
    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        coins = 0;
        health = 30;
        SetCoinText();
        SetLifeText();
        rb2d.freezeRotation = true;
    }
    void FixedUpdate(){
        float moveHorz = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorz, moveVert);
        movement.Normalize();
        rb2d.AddForce(movement * speed);
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);
            coins++;
            SetCoinText();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;
            SetLifeText();
            rb2d.AddForce(collision.relativeVelocity * knockback );
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void SetCoinText()
    {
        coinText.text = "Coins: " + coins.ToString();
    }
    
    void SetLifeText()
    {
        lifeText.text = "Life: " + health.ToString();
    }
}
