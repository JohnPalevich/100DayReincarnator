using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public int cooldown;
    public GameObject player;


    private float timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        timeStamp = 0;

    }

    // Update is called once per frame
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

    }
}
