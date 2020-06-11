using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerController : MonoBehaviour
{
    public GameObject player;
    public float speed;

    private Rigidbody2D rb2d;
    private DropTable dropTable;

    private Vector2 force;
    public float health;
    public float maxHealth;
    private Transform bar;
    private Transform hpBar;

    //Finds the player and sets up some basic information
    void Start()
    {
        hpBar = transform.Find("HealthBar");
        bar = hpBar.transform.Find("Bar");
        hpBar.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        force.x = 0;
        force.y = 0;
        rb2d.freezeRotation = true;

        dropTable = GetComponent<DropTable>();
    }

    //Pushes the object towards the player
    void FixedUpdate()
    {
        if(player == null)
        {
            return;
        }
        Vector2 playerPos = player.transform.position;
        float dirX = (transform.position.x - playerPos.x + force.x) * -1;
        float dirY = (transform.position.y - playerPos.y + force.y) * -1; 
        Vector2 movement = new Vector2(dirX, dirY);
        movement.Normalize();
        force = movement;
        rb2d.AddForce(movement * speed);
    }

    //When colliding, check to see if it with certain objects, if so...
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
            GameManager.instance.decreaseEnemiesAlive();
            dropTable.dropTable(pos);
        }
    }

    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f, 1f);
    }
}
