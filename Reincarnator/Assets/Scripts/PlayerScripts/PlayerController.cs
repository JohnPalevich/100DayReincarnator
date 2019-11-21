﻿using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetMouseButtonDown(0) && !thrown)
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
            DestroyObject(boomerang);
            updateBoomerang();
            thrown = false;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void throwBoomerang()
    {
        Vector2 spawnPos = findSlope(1.3f);
        boomerang = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
        boomerang.transform.parent = transform;
        Rigidbody2D boomRB2D = boomerang.GetComponent<Rigidbody2D>();
        float dirX = (transform.position.x - boomerang.transform.position.x) * -1;
        float dirY = (transform.position.y - boomerang.transform.position.y) * -1;
        Vector2 movement = new Vector2(dirX, dirY);
        movement.Normalize();
        boomRB2D.AddForce(movement * 500);
        thrown = true;
    }

    private Vector2 findSlope(float xOffset)
    {
        Vector3 mousePos = Input.mousePosition;//gets mouse postion
        mousePos = camera.ScreenToWorldPoint(mousePos);
        float x1 = transform.position.x;
        float x2 = mousePos.x;
        float y1 = transform.position.y;
        float y2 = mousePos.y;
        if (x1 < x2)
        {
            float xHold = x1;
            x1 = x2;
            x2 = xHold;
            float yHold = y1;
            y1 = y2;
            y2 = yHold;
        }
        float slope = (y2 - y1) / (x2 - x1);
        if (transform.position.x > mousePos.x)
        {
            xOffset *= -1;
        }
        float newX = transform.position.x + xOffset;
        float b = slope * transform.position.x * -1 + transform.position.y;
        float newY = slope * (newX) + b;
        return new Vector2(newX, newY);
    }

    public void updateBoomerang()
    {
        boomerang = prefab;
    }
}