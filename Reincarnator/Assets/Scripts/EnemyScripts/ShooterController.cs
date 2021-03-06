﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public float cooldown;
    public GameObject player;
    public GameObject bullet;
    public int bulletSpeed;
    public int speed;
    public float increment;
    private Transform bar;
    private Transform hpBar;

    private bool flip = false;
    private float timeStamp;
    private Rigidbody2D rb2d;
    private float health = 3f;
    private float maxHealth = 3f;
    private DropTable dropTable;
    void Start()
    {
        hpBar = transform.Find("HealthBar");
        bar = hpBar.transform.Find("Bar");
        hpBar.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        timeStamp = Time.time + cooldown;
        rb2d.freezeRotation = true;
        dropTable = GetComponent<DropTable>();
    }

    //Updates the timeStamp and runs away from the player if it gets too close.
    void FixedUpdate()
    {
        if (timeStamp <= Time.time)
        {
            Shoot();   
        }
        Vector2 dist = DistToPlayer(transform.position);
        if (dist.magnitude < 3)
        {
            dist.Normalize();
            rb2d.AddForce(dist * 30 * -1);
        }
        if(dist.x > 0 && !flip)
        {
            flip = true;
            transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
            if(hpBar != null)
            {
                hpBar.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else if (dist.x < 0 && flip)
        {
            flip = false;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            if(hpBar != null)
            {
                hpBar.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    //Tests to see if it collides with the players weapon.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Sword"))
        {
            if (health == maxHealth)
            {
                hpBar.gameObject.SetActive(true);
                hpBar.position = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f, transform.localPosition.z);
            }
            if (collision.gameObject.CompareTag("Boomerang"))
            {
                health -= PlayerController.player.bDamage();
            }
            else
            {
                health -= PlayerController.player.sDamage();
            }
            float f = health / maxHealth;
            SetSize(f);
        }
        if (health <= 0)
        {
            Vector3 pos = transform.position;
            Destroy(gameObject);
            GameManager.instance.decreaseEnemiesAlive();
            dropTable.dropTable(pos);
        }
    }

    //Method to shoot the player and set the cooldown.
    private void Shoot()
    {
        timeStamp = Time.time + cooldown;
        CreateBullet();
    }

    //Creates a bullet and shoots it towards the players
    private void CreateBullet()
    {
        Vector3 difference = new Vector3(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y, 0) * -1;
        difference.Normalize();
        Vector3 spawnPos = transform.position + difference;
        //Creates new bullet and sets it's parent to this object
        var myNewBullet = Instantiate(bullet, spawnPos, Quaternion.Euler(0f, 0f, 0f));

        //Initialized bullet's rigidbody, and gives it a direction to move in.
        Rigidbody2D rb2d = myNewBullet.GetComponent<Rigidbody2D>();
        Vector2 dist = DistToPlayer(spawnPos);
        dist.Normalize();
        rb2d.AddForce(dist * 100 * bulletSpeed);

        //Points the projectile towards the player object.
        Vector2 moveDirection = dist;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            myNewBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //Finds the distance to the player from current location.
    private Vector2 DistToPlayer(Vector3 pos)
    {
        Vector2 playerPos = player.transform.position;
        float dirX = (pos.x - playerPos.x) * -1;
        float dirY = (pos.y - playerPos.y) * -1;
        Vector2 vec = new Vector2(dirX, dirY);
        return vec;
    }
    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f);
    }
}
