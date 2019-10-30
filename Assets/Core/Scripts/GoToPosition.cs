using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPosition : MonoBehaviour
{
    public Transform pos;

    private void Update()
    {
        transform.position = pos.position;
        transform.localEulerAngles = new Vector3(0,pos.localEulerAngles.y,pos.localEulerAngles.z);
    }

}
