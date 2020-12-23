using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkorderUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timer, list, chances;

    private void Update() {
        timer.text = "NEXT ORDER: " + ((int)WorkorderManager.instance.newOrderTimer).ToString() + " SECONDS";

        list.text = "";
        foreach (WorkorderManager.Order od in WorkorderManager.instance.ordersToShow) {
            list.text += od.reqID + " x " + od.amount.ToString() + ", in " + ((int)od.duration).ToString() + " seconds\n";
        }

        chances.text = "You get " + GameManager.instance.chancesOfFail.ToString() + " more orders to f**k up :)";
    }
}
