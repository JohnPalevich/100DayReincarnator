using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject player;
    public int speed;

    void Start()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x,  player.transform.position.y);
        transform.LookAt(player.transform);
    }

    void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
}
