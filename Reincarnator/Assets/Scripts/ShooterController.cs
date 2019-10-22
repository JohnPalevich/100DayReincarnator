using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public int cooldown;
    public GameObject player;
    public GameObject bullet;
    public int bulletSpeed;


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
    }

    private void Shoot()
    {
        timeStamp = Time.time + cooldown;
        Vector3 pos = new Vector3(5, 0);

        CreateBullet(pos);
    }

    private void CreateBullet(Vector3 pos)
    {


/*        //Points the projectile towards the player object.
        Vector3 diff = player.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion.Euler(0f, 0f, rot_z);*/

        var myNewBullet = Instantiate(bullet);
        myNewBullet.transform.parent = gameObject.transform;


        //Initialized bullet's rigidbody, and gives it a direction to move in.
        Rigidbody2D rb2d = myNewBullet.GetComponent<Rigidbody2D>();
        Vector2 playerPos = player.transform.position;
        float dirX = (transform.position.x - playerPos.x) * -1;
        float dirY = (transform.position.y - playerPos.y) * -1;
        Vector2 movement = new Vector2(dirX, dirY);
        movement.Normalize();
        rb2d.AddForce(movement * 100 * bulletSpeed);
    }
}
