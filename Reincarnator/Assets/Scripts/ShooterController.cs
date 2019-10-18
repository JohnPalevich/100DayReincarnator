using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public int cooldown;
    public GameObject player;
    public GameObject bullet;

    private float timeStamp;
    private Rigidbody2D rb2d;
    private bool shot;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeStamp = 0;
        shot = false;
        rb2d.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (timeStamp <= Time.time && shot == false)
        {
            Shoot();   
        }
    }

    private void Shoot()
    {
        timeStamp = Time.time + cooldown;
        shot = true;
        Vector3 spawnLoc = transform.position;
        Instantiate(bullet);
    }
}
