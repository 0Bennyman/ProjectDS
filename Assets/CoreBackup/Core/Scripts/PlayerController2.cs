using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerController2 : MonoBehaviour
{

    public GameObject cam;

    public Rigidbody rb;

    private float walkSpeed;
    private float sprintSpeed;
    private float curMaxSpeed;

    public float force;

    public float speed;
    public float maxSpeed;

    public Animator anim;

    private bool lockOn;

    public GameObject enemyLockOn;

    public GameObject ourBodyLockOn;

    public CinemachineFreeLook camLook;

    private float maxVelSpeed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        walkSpeed = speed;
        sprintSpeed = speed * 2;
        curMaxSpeed = maxSpeed;
        maxVelSpeed = 4;
    }


    private void Update()
    {

        anim.SetFloat("YVelocity", rb.velocity.y);

        if (rb.velocity.magnitude > .01 && rb.velocity.y>-.5f&&rb.velocity.y<.5f)
        {
            //transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            var q = Quaternion.LookRotation(rb.velocity, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 300 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Running", true);
            speed = sprintSpeed;
            maxVelSpeed = 4.5f;
        }
        else
        {
            anim.SetBool("Running", false);
            speed = walkSpeed;
            maxVelSpeed = 4;
        }

        if (curMaxSpeed > maxSpeed)
        {
            curMaxSpeed -= 12 * Time.deltaTime;
        }
        else
        {
            curMaxSpeed = maxSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lockOn = !lockOn;
        }

        if (lockOn)
        {
            camLook.LookAt = enemyLockOn.transform;
        }
        else
        {
            camLook.LookAt = ourBodyLockOn.transform;
        }


    }


    void FixedUpdate()
    {

        if (curMaxSpeed <= maxSpeed)
        {
            if (Input.GetKeyDown(KeyCode.F) && rb.velocity.magnitude>.5f)
            {
                rb.AddForce(transform.forward * speed * 8, ForceMode.Impulse);
                curMaxSpeed = maxSpeed * 8;
                StartCoroutine(Dash());
                //rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(cam.transform.forward * speed, ForceMode.Impulse);
                // rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
                anim.SetBool("Moving", true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(-cam.transform.forward * speed, ForceMode.Impulse);
                // rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
                anim.SetBool("Moving", true);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(cam.transform.right * speed, ForceMode.Impulse);
                // rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
                anim.SetBool("Moving", true);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(-cam.transform.right * speed, ForceMode.Impulse);
                //rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
                anim.SetBool("Moving", true);
            }

            if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > -.5f && rb.velocity.y < .5f)
            {
                rb.AddForce(cam.transform.up * force * 6, ForceMode.Impulse);
            }

        }

        print(rb.velocity.x);

        float tempY = rb.velocity.y;
        float tempX = rb.velocity.x;
        float tempZ = rb.velocity.z;

        if (tempX > maxVelSpeed)
        {
            tempX = maxVelSpeed;
        }else if (tempX < -maxVelSpeed)
        {
            tempX = -maxVelSpeed;
        }

        if (tempZ > maxVelSpeed)
        {
            tempZ = maxVelSpeed;
        }
        else if (tempZ < -maxVelSpeed)
        {
            tempZ = -maxVelSpeed;
        }

        if (rb.velocity.magnitude > curMaxSpeed)
        {
            //rb.velocity = rb.velocity.normalized * curMaxSpeed;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
        }


        rb.velocity = new Vector3(tempX, tempY, tempZ);


        //if (rb.velocity.y > -.5f && rb.velocity.y < .5f)
        //{
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, curMaxSpeed);
        //}

        if (!Input.GetKey(KeyCode.A)&& !Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.S)&& !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Moving", false);
        }
    }


    IEnumerator Dash()
    {
        yield return new WaitForSeconds(.45f);
        curMaxSpeed = maxSpeed;
    }
}
