using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeManager : MonoBehaviour
{
    public Transform axePrefab;
    public Transform axeInstance;
    public Transform axeSpawnPoint;

    // Update is called once per frame
    void Update()
    {
        if (axeInstance == null)
        {
            Debug.Log("No axe found");
            axeInstance = Instantiate(axePrefab);
            axeInstance.position = axeSpawnPoint.position;
            axeInstance.GetComponent<Rigidbody>().AddForce(axeSpawnPoint.forward * 1000f);
        }
    }
}
