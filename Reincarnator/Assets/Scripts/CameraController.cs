using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public static CameraController camera = null;
    private Vector3 offset;

    // Start is called before the first frame update
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

    public void setOffset()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;    
    }
}
