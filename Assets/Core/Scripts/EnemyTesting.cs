using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour
{

    private Animator anim;

    public GameObject melee;

    public GameObject playerLock;//debug

    public int Health;

    public int castLength; //say he only attacks a few times before cast runs out and he re-casts

    private GameObject player;

    private float delay;

    private Vector3 playerPos;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        melee.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            anim.SetBool("Hit",true);
            anim.SetBool("Idle", false);
        }
    }

    private void Update()
    {

        if (Health <= 0)
        {
            anim.SetBool("Die", true);
            anim.SetBool("Idle", false);
        }

        if (anim.GetBool("Running")||anim.GetBool("Locked"))
        {
            transform.position = Vector3.MoveTowards(transform.position,playerPos,5*Time.deltaTime);
        }

        if (anim.GetBool("Locked"))
        {
            return;
        }

        AIStuffs();

        playerPos = player.transform.transform.position;
        transform.LookAt(playerLock.transform.position);



    }


    private void AIStuffs()
    {
        //THIS IS BADLY DONE AND RUSHED OKAY DON'T JUDGE

        if (delay > 0)
        {
            delay -= 1 * Time.deltaTime;
            return;
        }

        if (castLength<=0 && Vector3.Distance(transform.position, player.transform.position) > 3)
        {
            //Cast
            anim.SetBool("Casting",true);
            anim.SetBool("Idle", false);
        }
        else if (Vector3.Distance(transform.position, player.transform.position) <= 3)
        {
            //Kick
            anim.SetBool("Kick", true);
            anim.SetBool("Idle", false);
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > 3 && anim.GetBool("Idle"))
        {
            if (!anim.GetBool("Running"))
            {
                anim.SetBool("Running", true);
                //anim.SetBool("Locked", true);
                anim.SetBool("Idle", false);
                delay = 1000;
                StartCoroutine(runAttack());
            }
        }
    }


    IEnumerator runAttack()
    {
        yield return new WaitForSeconds(2f);

        anim.SetBool("Attack",true);
        anim.SetBool("Locked", true);
        castLength -= 1;
        delay = 2;

    }

}
