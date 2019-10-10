using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    public GameObject player;
    public float speed;

    private Rigidbody2D rb2d;
    private Vector2 force;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        force.x = 0;
        force.y = 0;
        rb2d.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if(player == null)
        {
            return;
        }
        Vector2 playerPos = player.transform.position;
        float dirX = (transform.position.x - playerPos.x + force.x) * -1;
        float dirY = (transform.position.y - playerPos.y + force.y) * -1; 
        Vector2 movement = new Vector2(dirX, dirY);
        movement.Normalize();
        force = movement;
        rb2d.AddForce(movement * speed);
    }
}
