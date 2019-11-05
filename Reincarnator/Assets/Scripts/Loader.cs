using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject camera;
    public GameObject gameManager;
    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        if (CameraController.camera == null)
        {
            Instantiate(camera);
        }
    }

}