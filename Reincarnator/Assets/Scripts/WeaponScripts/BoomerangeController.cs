using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangeController : MonoBehaviour
{
    private GameObject player;
    
    private Rigidbody2D rb2d;
    private bool returning;
    private Vector2 force;
    private Transform initPos;

    //Information for reseting the physics of the boomerang
    private float mass;
    private float linDrag;
    private float angDrag;

    //Sets up information for the boomerang to use
    void Start()
    {
        //Tells the boomerang to not return
        returning = false;

        //Finds the player so it can return to it later.
        player = GameObject.Find("Player");

        rb2d = GetComponent<Rigidbody2D>();
        force.x = 0;
        force.y = 0;

        //Basically removes all friction that could slow the boomerang
        mass = rb2d.mass;
        rb2d.mass = 0;
        linDrag = rb2d.drag;
        rb2d.drag = 0;
        angDrag = rb2d.angularDrag;
        rb2d.angularDrag = 0;
    }

    //Track player down if returning.
    void Update()
    {
        if (returning){
            //Code that tracks the player down.
            Vector2 playerPos = player.transform.position;
            float dirX = (transform.position.x - playerPos.x) * -1;
            float dirY = (transform.position.y - playerPos.y) * -1;
            Vector2 movement = new Vector2(dirX, dirY);
            movement.Normalize();
            force = movement;
            rb2d.AddForce(movement * 20);
        }
    }

    //If collides with certain objects, then do...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Map")
            || collision.gameObject.CompareTag("Bullets") && !returning)
        {
            setReturning();
        }
    }

    //Gets the object's physics ready for returning.
    private void setReturning()
    {
        //resets the rigidbody information so it can do cool boomerang things
        rb2d.mass = mass;
        rb2d.drag = linDrag;
        rb2d.angularDrag = angDrag;
        returning = true;
    }
}