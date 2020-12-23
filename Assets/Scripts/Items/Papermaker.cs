using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Papermaker : MonoBehaviour
{
    [SerializeField]
    private Transform OutputPoint;

    [System.Serializable]
    public class ListWrapper
    {
        public string inputItemId;
        public List<GameObject> outputs;
    }

    private List<Item> inputs = new List<Item>();
    Item currentInput = null;
    [SerializeField]
    private GameObject product1;
    [SerializeField]
    private GameObject product2;
    [SerializeField]
    private List<ListWrapper> inputsToProducts;
    [SerializeField]
    private float taskTime;
    private float timer;
    private bool machineActive;
    private List<GameObject> currentOutputs;

    private void Update()
    {
        if (machineActive)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                int count = 0;
                foreach(GameObject obj in currentOutputs)
                {
                    GameObject.Instantiate(obj, OutputPoint.position + OutputPoint.forward * count, Quaternion.identity);
                }
                machineActive = false;
            }
        }
        else
        {
            if (inputs.Count > 0)
            {
                foreach(ListWrapper item in inputsToProducts)
                {
                    if (item.inputItemId == inputs[0].itemId)
                    {
                        currentOutputs = new List<GameObject>(item.outputs);
                        // Turn on the machine automatically
                        // Take the first input
                        currentInput = inputs[0];
                        inputs.RemoveAt(0);
                        Destroy(currentInput.gameObject);
                        machineActive = true;
                        timer = taskTime;
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Item target = collision.gameObject.GetComponent<Item>();
        if (target != null)
        {
            inputs.Add(target);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Item target = collision.gameObject.GetComponent<Item>();
        if (target != null)
        {
            inputs.Remove(target);
        }
    }
}
