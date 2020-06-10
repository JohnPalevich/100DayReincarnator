using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    public GameObject coin;
    public GameObject powerup1;
    public GameObject powerup2;

    public void dropTable(Vector3 pos)
    {
        System.Random rnd = new System.Random();
        int num1 = rnd.Next(0, 100);
        if(num1 < 100)
        {
            int num2 = rnd.Next(0, 100);
            if(num2 < 50)
            {
                Instantiate(coin, pos, Quaternion.Euler(0f, 0f, 0f));
            }
            else if(num2 >=50 && num2 < 75)
            {
                Instantiate(powerup1, pos, Quaternion.Euler(0f, 0f, 0f));
            }
            else
            {
                Instantiate(powerup2, pos, Quaternion.Euler(0f, 0f, 0f));
            }
        }
    }
}
