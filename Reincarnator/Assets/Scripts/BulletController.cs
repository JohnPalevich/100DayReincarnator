using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject player;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x,  player.transform.position.y);
        transform.rotation = Quaternion.LookRotation(playerPos);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
