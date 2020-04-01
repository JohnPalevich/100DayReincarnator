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
    private float health = 5;
    private float maxHealth = 5;
    private Transform hpBar;
    private Transform bar;
    private Vector2 movement;
    private Animator animator;
    //Sets up basic information.
    void Start()
    {
        hpBar = transform.Find("HealthBar");
        bar = hpBar.transform.Find("Bar");
        hpBar.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        charging = false;
        rb2d.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    //Checking to see if it needs to start charging
    void FixedUpdate()
    {
        if (timeStamp <= Time.time && !charging)
        {
            Vector2 playerPos = player.transform.position;
            float dirX = (transform.position.x - playerPos.x) * -1;
            float dirY = (transform.position.y - playerPos.y) * -1;
            movement = new Vector2(dirX, dirY);
            movement.Normalize();
            movement = movement * speed;
            rb2d.AddForce(movement);
            charging = true;
            animator.Play("BullCharge", 0 , 0f);
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
            if (health == maxHealth)
            {
                hpBar.gameObject.SetActive(true);
                hpBar.position = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f, transform.localPosition.z);
            }
            health--;
            float f = health / maxHealth;
            SetSize(f);
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
        animator.Play("BullRefresh", 0 , 0f);
    }

    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f, 1f);
    }
}
