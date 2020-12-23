using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeManager : MonoBehaviour
{
    public Transform axePrefab;
    public Transform axeInstance;
    public Transform axeSpawnPoint;
    public float throwForce = 1000f;

    // Update is called once per frame
    void Update()
    {
        if (axeInstance == null)
        {
            axeInstance = Instantiate(axePrefab);
            axeInstance.position = axeSpawnPoint.position;
            axeInstance.GetComponent<Rigidbody>().AddForce(axeSpawnPoint.forward * throwForce);
        }
    }
}
