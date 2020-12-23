using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cameraTransform;

    private void Start() {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
