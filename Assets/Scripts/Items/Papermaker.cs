﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Papermaker : MonoBehaviour
{
    [SerializeField]
    private Transform OutputPoint;

    private List<Item> inputs = new List<Item>();
    Item currentInput = null;
    [SerializeField]
    private GameObject product1;
    [SerializeField]
    private GameObject product2;
    [SerializeField]
    private float taskTime;
    private float timer;
    private bool machineActive;

    private void Update()
    {
        if (machineActive)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                // TODO: Check if the input is indeed wood
                GameObject.Instantiate(product1, OutputPoint.position, Quaternion.identity);
                GameObject.Instantiate(product2, OutputPoint.position + OutputPoint.forward, Quaternion.identity);
                machineActive = false;
            }
        }
        else
        {
            if (inputs.Count > 0)
            {
                if (inputs[0].itemId == "Log")
                {
                    // Turn on the machine automatically
                    // Take the first input
                    currentInput = inputs[0];
                    inputs.RemoveAt(0);
                    Destroy(currentInput.gameObject);
                    machineActive = true;
                    timer = taskTime;
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
