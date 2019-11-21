using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangeController : MonoBehaviour
{
    public GameObject player;
    
    private Rigidbody2D rb2d;
    private bool returning;
    private Vector3 dist;
    private Vector2 force;
    // Start is called before the first frame update
    void Start()
    {
        returning = false;
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        force.x = 0;
        force.y = 0;
        //Vector3 dist = new Vector3(transform.x + )
    }

    // Update is called once per frame
    void Update()
    {
        if (returning){
            Vector2 playerPos = player.transform.position;
            float dirX = (transform.position.x - playerPos.x) * -1;
            float dirY = (transform.position.y - playerPos.y) * -1;
            Vector2 movement = new Vector2(dirX, dirY);
            movement.Normalize();
            force = movement;
            rb2d.AddForce(movement * 20);
        }
        else
        {
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            returning = true;
        }
    }
}
