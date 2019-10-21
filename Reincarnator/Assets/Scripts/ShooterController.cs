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

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeStamp = 0;
        rb2d.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (timeStamp <= Time.time)
        {
            Shoot();   
        }
    }

    private void Shoot()
    {
        timeStamp = Time.time + cooldown;
        Vector3 pos = new Vector3(5, 0);
        var myNewBullet = Instantiate(bullet, pos, transform.rotation);
        myNewBullet.transform.parent = gameObject.transform;
    }
}
