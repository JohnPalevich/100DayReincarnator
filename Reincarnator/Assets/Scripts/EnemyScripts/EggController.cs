using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public GameObject spawn;
    public float health;
    public float maxHealth;
    
    private Transform bar;
    private Transform hpBar;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = transform.Find("HealthBar");
        bar = hpBar.transform.Find("Bar");
        hpBar.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Bullets") || collision.gameObject.CompareTag("Sword"))
        {
            if (health == maxHealth)
            {
                hpBar.gameObject.SetActive(true);
                hpBar.position = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f, transform.localPosition.z);
            }
            if (collision.gameObject.CompareTag("Bullets"))
            {
                health -= 1;
            }
            else if (collision.gameObject.CompareTag("Boomerang"))
            {
                health -= PlayerController.player.bDamage();
            }
            else
            {
                health -= PlayerController.player.sDamage();
            }
            float f = health / maxHealth;
            SetSize(f);
            Debug.Log("Health: " + health);
        }
        if (health <= 0)
        {
            Vector3 pos = transform.position;
            Destroy(gameObject);
            spawnOnDeath(pos);
            GameManager.instance.decreaseEnemiesAlive();
        }
    }

    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f, 1f);
    }

    private void spawnOnDeath(Vector3 pos)
    {
        System.Random rnd = new System.Random();
        for(int i = 0; i < rnd.Next(2,3); i++)
        {
            Vector3 newPos = new Vector3(pos.x, pos.y, pos.z) ;
            Instantiate(spawn, pos, Quaternion.Euler(0f, 0f, 0f));
            GameManager.instance.decreaseEnemiesAlive();
        }
    }

}
