﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTaker: MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        ToyProduct toy = other.gameObject.GetComponent<ToyProduct>();
        if (toy == null) {
            return;
        }

        // submit the toy to work order system
        WorkorderManager.instance.NewSubmission(toy);
    }
}
