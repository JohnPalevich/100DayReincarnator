using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
   
    public int speed;

    //private GameObject player;
    //private Rigidbody2D rb2d;
    void Start()
    {
         
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Background"))
        {
            Destroy(gameObject);
        }

    }
}
