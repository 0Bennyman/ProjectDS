using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private GameObject stuckTo;
    private Transform pos;

    private Rigidbody rb;
    private BoxCollider box;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        box = gameObject.GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        stuckTo = collision.gameObject;
        transform.parent = stuckTo.transform;
        pos = transform;
        Destroy(rb);
        Destroy(box);
        Destroy(this);
    }



}
