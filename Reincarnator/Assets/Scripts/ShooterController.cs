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

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeStamp = 0;
        rb2d.freezeRotation = true;
    }

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
            rb2d.AddForce(dist * 70 * -1);
        }
    }

    private void Shoot()
    {
        timeStamp = Time.time + cooldown;
        Vector3 pos = new Vector3(5, 0);

        CreateBullet(ShootPosition());
    }

    private void CreateBullet(Vector3 pos)
    {
        //Points the projectile towards the player object.
        Vector3 diff = player.transform.position - pos;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        var myNewBullet = Instantiate(bullet, pos, Quaternion.Euler(0f, 0f, rot_z - 12));
        myNewBullet.transform.parent = gameObject.transform;

        //Initialized bullet's rigidbody, and gives it a direction to move in.
        Rigidbody2D rb2d = myNewBullet.GetComponent<Rigidbody2D>();
        Vector2 movement = DistToPlayer(pos);
        movement.Normalize();
        rb2d.AddForce(movement * 100 * bulletSpeed);
    }

    private Vector2 DistToPlayer(Vector3 pos)
    {
        Vector2 playerPos = player.transform.position;
        float dirX = (pos.x - playerPos.x) * -1;
        float dirY = (pos.y - playerPos.y) * -1;
        Vector2 vec = new Vector2(dirX, dirY);
        return vec;
    }

    private Vector3 ShootPosition()
    {
        Vector2 dist = DistToPlayer(transform.position);
        if(dist.x > 0 && dist.y > 0)
        {
            return new Vector3(transform.position.x + increment, transform.position.y + increment);
        }
        else if(dist.x > 0 && dist.y < 0)
        {
            return new Vector3(transform.position.x + increment, transform.position.y - increment);
        }
        else if (dist.x < 0 && dist.y < 0)
        {
            return new Vector3(transform.position.x - increment, transform.position.y - increment);
        }
        return new Vector3(transform.position.x - increment, transform.position.y + increment);
    }
}
