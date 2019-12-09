using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerController player;
    public static CameraController camera = null;
    private Vector3 offset;

    //Ensures there is only one camera, and makes the camera centered on the player.
    void Start()
    {
        if (camera == null)
        {
            camera = this;
        }
        else if (camera != this)
        {
            Destroy(gameObject);
        }
        offset = transform.position - player.transform.position;
    }

    //Keeps the camera with the player.
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;    
    }
}
