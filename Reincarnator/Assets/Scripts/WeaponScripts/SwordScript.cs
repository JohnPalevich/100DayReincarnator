using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{

    private Animator animator;
    public int swordAtk = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("SwordSwing", 0, 0f);
        //PlayerController.player.setSDamage(swordAtk);
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            PlayerController.player.finSwing();
            Object.Destroy(gameObject);
        }
    }

}
