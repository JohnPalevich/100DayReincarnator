﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Map") 
            || collision.gameObject.CompareTag("Boomerang"))
        {
            Destroy(gameObject);
        }

    }
}
