using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public float cooldown;
    public int speed;
    public GameObject player;

    private bool charging;
    private Rigidbody2D rb2d;
    private DropTable dropTable;
    private float timeStamp;
    private float health = 5;
    private float maxHealth = 5;
    private Transform hpBar;
    private Transform bar;
    private Vector2 movement;
    private Animator animator;
    private bool flip = false;

    //Sets up basic information.
    void Start()
    {
        hpBar = transform.Find("HealthBar");
        bar = hpBar.transform.Find("Bar");
        hpBar.gameObject.SetActive(false);
        player = GameObject.Find("Player");
        rb2d = GetComponent<Rigidbody2D>();
        charging = false;
        rb2d.freezeRotation = true;
        animator = GetComponent<Animator>();
        dropTable = GetComponent<DropTable>();
    }

    //Checking to see if it needs to start charging
    void FixedUpdate()
    {
        if (timeStamp <= Time.time && !charging)
        {
            Vector2 playerPos = player.transform.position;
            float dirX = (transform.position.x - playerPos.x) * -1;
            float dirY = (transform.position.y - playerPos.y) * -1;
            movement = new Vector2(dirX, dirY);
            movement.Normalize();
            movement = movement * speed;
            rb2d.AddForce(movement);
            charging = true;
            setDirection(transform.position);
            animator.Play("BullCharge", 0 , 0f);
        }
    }

    //Executes some checks when colliding with other objects.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Player"))
        {
            stopMovement();
        }
        else if (collision.gameObject.CompareTag("Boomerang") || collision.gameObject.CompareTag("Bullets") || collision.gameObject.CompareTag("Sword"))
        {
            stopMovement();
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

    private void setDirection(Vector3 pos)
    {
        Vector2 playerPos = player.transform.position;
        float dirX = (pos.x - playerPos.x) * -1;
        float dirY = (pos.y - playerPos.y) * -1;
        Vector2 dist = new Vector2(dirX, dirY);
        if (dist.x > 0 && !flip)
        {
            flip = true;
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            if (hpBar != null)
            {
                hpBar.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else if (dist.x < 0 && flip)
        {
            flip = false;
            transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
            if (hpBar != null)
            {
                hpBar.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    //Code to set cooldown on itself and stop itself from moving.
    private void stopMovement()
    {
        rb2d.velocity = Vector2.zero;
        charging = false;
        timeStamp = Time.time + cooldown;
        animator.Play("BullRefresh", 0 , 0f);
    }

    public void SetSize(float hpLeft)
    {
        bar.localScale = new Vector3(hpLeft, 1f, 1f);
    }
}
