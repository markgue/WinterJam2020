using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cameraTransform;


    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
