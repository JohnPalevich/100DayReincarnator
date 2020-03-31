using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    //Checks to see if the bullet has collided with certain objects, if so, destroy itself.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullets"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Bullets"))
        {
            Destroy(gameObject);
        }
    }
}
