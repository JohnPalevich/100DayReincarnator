using System.Collections;
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
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.player.gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        charging = false;
        rb2d.freezeRotation = true;
    }

    // Update is called once per frame
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            stopMovement();
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            stopMovement();
        }
    }

    private void stopMovement()
    {
        rb2d.velocity = Vector2.zero;
        charging = false;
        timeStamp = Time.time + cooldown;
    }
}
