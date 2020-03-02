using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    private Transform bar; 

    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f);
    }
}
