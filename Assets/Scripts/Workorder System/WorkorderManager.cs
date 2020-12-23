using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkorderManager : MonoBehaviour
{
    public static WorkorderManager instance;

    // parameters 
    [SerializeField]
    private float orderInterval; // interval between each arrival of workorder

    // interface with player
    [SerializeField]
    private OrderGiver giver;
    [SerializeField]
    private OrderTaker taker;
    [SerializeField]
    private GameObject orderUI;

    // lifecycle management
    private float newOrderTimer;

    // work order data structure
    public struct Order {
        public Order(string[] ids, int[] counts, float dur, float stTime = 0) {
            reqID = ids;
            amount = counts;
            duration = dur;
            startTime = stTime;
        }

        public string[] reqID;
        public int[] amount;
        public float duration;
        public float startTime;
    }
    private List<Order> orderList = new List<Order>();


    // system messages ///////////////////////////////////////////////////////////
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Update() {
        // TODO: check work order completion

        // check order deadline
        foreach (Order od in orderList) {
            if (Time.time - od.startTime > od.duration) {
                GameManager.instance.OrderOverdue();
            }
        }
    }


    // work order generation /////////////////////////////////////////////////////
    private float NextDuration() { 
        return orderInterval;
    }

    private void RandomWorkOrder() {

    }
}
