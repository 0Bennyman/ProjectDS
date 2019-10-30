using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour
{

    private Animator anim;

    public GameObject melee;

    public GameObject playerLock;//debug

    public int Health;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        melee.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            anim.SetBool("Hit",true);
        }
    }

    private void Update()
    {
        transform.LookAt(playerLock.transform.position);

        if (Health <= 0)
        {
            anim.SetBool("Die",true);
        }

    }


}
