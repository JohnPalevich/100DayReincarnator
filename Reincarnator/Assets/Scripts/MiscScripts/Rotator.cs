using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public int rotateAmount;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateAmount) * Time.deltaTime);
    }
}
