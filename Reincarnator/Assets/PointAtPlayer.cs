using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtPlayer : MonoBehaviour
{
    //private Quaternion original;
    // Start is called before the first frame update
    void Start()
    {
        //original = transform.GetChild(0).rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = PlayerController.player.transform.position;
        float dirX = (transform.position.x - playerPos.x) * -1;
        float dirY = (transform.position.y - playerPos.y) * -1;
        //Points the projectile towards the player object.
        Vector2 moveDirection = new Vector2(dirX, dirY);
        moveDirection.Normalize();
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle+90f, Vector3.forward);
            //transform.GetChild(0).transform.rotation = original;
        }
    }
}
