using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject player;
    public int speed;

    private Rigidbody2D rb2d;
    void Start()
    {
        //point the projectile towards the enemy.
        Vector3 diff = player.transform.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        //Initialized bullet's rigidbody, and gives it a direction to move in.
        rb2d = GetComponent<Rigidbody2D>();
        Vector2 playerPos = player.transform.position;
        float dirX = (transform.position.x - playerPos.x) * -1;
        float dirY = (transform.position.y - playerPos.y) * -1;
        Vector2 movement = new Vector2(dirX, dirY);
        movement.Normalize();
        rb2d.AddForce(movement * speed);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
