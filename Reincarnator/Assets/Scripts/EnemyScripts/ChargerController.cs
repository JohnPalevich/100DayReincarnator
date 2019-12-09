﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public float cooldown;
    public int speed;
    public GameObject player;

    private bool charging;
    private Rigidbody2D rb2d;
    private float timeStamp;
    private int health = 5;
    
    //Sets up basic information.
    void Start()
    {
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        charging = false;
        rb2d.freezeRotation = true;
    }

    //Checking to see if it needs to start charging
    void FixedUpdate()
    {
        if (timeStamp <= Time.time && !charging)
        {
            Vector2 playerPos = player.transform.position;
            float dirX = (transform.position.x - playerPos.x) * -1;
            float dirY = (transform.position.y - playerPos.y) * -1;
            Vector2 movement = new Vector2(dirX, dirY);
            movement.Normalize();
            rb2d.AddForce(movement * speed);
            charging = true;
        }
    }

    //Executes some checks when colliding with other objects.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Player"))
        {
            stopMovement();
        }
        else if (collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Bullets"))
        {
            stopMovement();
            health--;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    //Code to set cooldown on itself and stop itself from moving.
    private void stopMovement()
    {
        rb2d.velocity = Vector2.zero;
        charging = false;
        timeStamp = Time.time + cooldown;
    }
}
