using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public int cooldown;
    public GameObject player;
    public GameObject bullet;
    public int bulletSpeed;
    public int speed;
    public float increment;

    private float timeStamp;
    private Rigidbody2D rb2d;
    private int health = 3;

    void Start()
    {
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        timeStamp = 0;
        rb2d.freezeRotation = true;
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
    }
    
    //Tests to see if it collides with the players weapon.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Bullets"))
        {
            health--;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
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
        myNewBullet.transform.parent = gameObject.transform;

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

}
