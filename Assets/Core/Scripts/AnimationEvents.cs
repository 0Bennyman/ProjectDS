using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    private Animator anim;
    private PlayerController control;
    private EnemyTesting enemy;


    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        control = gameObject.GetComponent<PlayerController>();
        enemy = gameObject.GetComponent<EnemyTesting>();
    }


    public void ClearFlagsEnemy()
    {
        anim.SetBool("Hit", false);
        //anim.SetBool("Attack", false);
        //anim.SetBool("Roll", false);
    }

    public void SetMeleeActive()
    {
        if (enemy)
        {
            enemy.melee.SetActive(!enemy.melee.activeSelf);
        }
        else
        {
            control.melee.SetActive(!control.melee.activeSelf);
        }

    }

    public void FinishLadderClimbAnim()
    {
        control.gameObject.transform.position = control.ladderPos.transform.position;
        control.lockMovement = false;
        control.rb.useGravity = true;
        control.StopAllCoroutines();
    }

    public void Damage()
    {
        //maybe make a dedicated script to handle health so it's the same across both enemy and player

        if (enemy)
        {
            enemy.Health -= 1;
        }
        else
        {
            control.Health -= 1;
        }

    }

    public void killMe()
    {
        Destroy(gameObject);
    }

    public void Lock()
    {
        if (enemy)
        {
            //anim.SetBool("Locked", true);
            //anim.SetBool("Running", false);
        }
    }

    public void Casted()
    {
        enemy.castLength = 3;
        anim.SetBool("Casting", false);
    }

    public void ClearFlags()
    {
        if (enemy)
        {
            anim.SetBool("Locked", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Locked", false);
            anim.SetBool("Running", false);
        }

        anim.SetBool("Hit", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Roll", false);
        anim.SetBool("Kick", false);

        if (control)
        {
            control.lockMovement = false;
            control.StartCoroutine(control.DelayAction());
        }

    }

    public void AddForce()
    {
        control.Dodge();
    }

}
