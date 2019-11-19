using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    //This is the removed one without jumping and for darksoulz

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

    public CinemachineVirtualCamera lockOnCam;

    private float maxVelSpeed;

    public bool lockMovement;

    private bool lockedActions;

    public GameObject melee;

    public int Health;

    [HideInInspector]
    public GameObject ladderPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            anim.SetBool("Hit",true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ladder" && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("ClimbLadder", true);
        }

        if (other.tag == "LadderTop" && anim.GetBool("ClimbLadder"))    
        {
            //anim.SetBool("ClimbOffTop",true); //Use for detailed getting on and off
            anim.SetBool("ClimbLadder", false);
            lockMovement = true;
            rb.useGravity = false;
            StartCoroutine(FinishLadderClimb(other.gameObject.GetComponent<Ladderz>().ladderPoint));
            //transform.position = other.gameObject.GetComponent<Ladderz>().ladderPoint.transform.position;
            //rb.useGravity = true;
        }
    }


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        walkSpeed = speed;
        sprintSpeed = speed * 2;
        curMaxSpeed = maxSpeed;
        maxVelSpeed = 4;

        melee.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    IEnumerator FinishLadderClimb(GameObject point)
    {
        yield return new WaitForSeconds(.03f);
        //transform.position = Vector3.MoveTowards(transform.position,point.transform.position,2.8f*Time.deltaTime); REMOVEDe
        ladderPos = point;

        if (Vector3.Distance(transform.position, point.transform.position) > .1f)
        {
            //StartCoroutine(FinishLadderClimb(point));
            //lockMovement = true;
            StartCoroutine(FinishLadderClimb(point));
        }
        else
        {
            //lockMovement = false;
            //rb.useGravity = true;
        }


    }


    private void Update()
    {

        if (lockMovement)
        {
            return;
        }

        if (anim.GetBool("ClimbLadder"))
        {
            ClimbingControls();
            rb.useGravity = false; //Use this when getting on ladder
            return;
        }

        if (rb.velocity.magnitude > .01 && rb.velocity.y>-.5f&&rb.velocity.y<.5f && anim.GetBool("Moving"))
        {
            //transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            var q = Quaternion.LookRotation(rb.velocity, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 800 * Time.deltaTime); //300 was og value
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

            if (lockOn)
            {
                camLook.enabled = false;
                lockOnCam.enabled = true;
                lockOnCam.LookAt = enemyLockOn.transform;
                //camLook.LookAt = enemyLockOn.transform;
            }
            else
            {
                camLook.enabled = true;
                lockOnCam.enabled = false;
                //camLook.LookAt = ourBodyLockOn.transform;
            }

        }

        if (lockOn)
        {
            lockOnCam.transform.LookAt(enemyLockOn.transform.position);
        }

        AnimationControls();
    }


    private void ClimbingControls()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up*.8f * Time.deltaTime);
        }
    }

    public IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(.2f);
        lockedActions = false;

    }


    private void AnimationControls()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !lockedActions)
        {
            anim.SetBool("Attack", true);
            transform.LookAt(enemyLockOn.transform.position);
            lockMovement = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !lockedActions)
        {
            anim.SetBool("Roll", true);
            lockMovement = true;
            lockedActions = true;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            anim.SetBool("Block", true);
        }
        else
        {
            anim.SetBool("Block", false);
        }
    }


    public void Dodge()
    {
        rb.AddForce(transform.forward * 40, ForceMode.Impulse);
    }




    void FixedUpdate()
    {

        if (lockMovement|| anim.GetBool("ClimbLadder"))
        {
            return;
        }

        if (curMaxSpeed <= maxSpeed && !anim.GetBool("Block"))
        {

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
            if (lockOnCam.enabled)
            {
                transform.LookAt(enemyLockOn.transform.position);
            }

        }
    }
}
