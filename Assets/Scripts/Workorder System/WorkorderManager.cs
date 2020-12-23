using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkorderManager : MonoBehaviour
{
    public static WorkorderManager instance;

    // parameters 
    [SerializeField]
    private float orderInterval, orderDuration; // interval between each arrival of workorder
    [SerializeField]
    private string[] productsToOrder; // array of product names

    // interfaces
    [SerializeField]
    private GameObject orderUI;

    // lifecycle management
    public float newOrderTimer;

    // work order data structure
    public struct Order {
        public Order(string ids, int counts, float dur, float stTime = 0) {
            reqID = ids;
            amount = counts;
            duration = dur;
            startTime = stTime;
        }

        public string reqID;
        public int amount;
        public float duration;
        public float startTime;
    }
    private Dictionary<string, List<Order>> workLists = new Dictionary<string, List<Order>>();
    public List<Order> ordersToShow = new List<Order>();


    // system messages ///////////////////////////////////////////////////////////
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        newOrderTimer = orderInterval;
    }

    private void Update() {
        // interval update
        if (newOrderTimer <= 0) {
            newOrderTimer = NextInterval();
            
            // create new order
            Order[] newOrder = NewOrder();

            // update lists
            foreach (Order temp in newOrder) {
                // UI will display orders in default order
                ordersToShow.Add(temp);
                if (workLists.ContainsKey(temp.reqID)) {
                    // add to existing list
                    workLists[temp.reqID].Add(temp);
                }
                else {
                    // create new list for this type
                    workLists.Add(temp.reqID, new List<Order>(){temp});
                }
            }
        }
        else {
            // update timer
            newOrderTimer -= Time.deltaTime;
        }
        
        // check order deadline
        foreach (KeyValuePair<string, List<Order>> list in workLists) {
            foreach (Order od in list.Value) {
                if (Time.time - od.startTime > od.duration) {
                    GameManager.instance.OrderOverdue();
                    list.Value.Remove(od);
                    ordersToShow.Remove(od);
                }
            }
        }
    }


    // work order generation /////////////////////////////////////////////////////
    private float NextInterval() { // in case the coming time is designed to be faster and faster
        return orderInterval;
    }

    private Order[] NewOrder() {
        // the defaults here are for single item orders
        Order temp = new Order(productsToOrder[Random.Range(0, productsToOrder.Length)], 1, orderDuration, Time.time);
        return new Order[1] { temp };
    }


    // work order submission /////////////////////////////////////////////////////
    public void NewSubmission(ToyProduct newSub) {
        // TODO: please make sure all no later order dues sooner than earlier orders because I'm only checking the orders in default order
        
        // if the toy is not required anywhere, it'll just stay where it is
        if (workLists.ContainsKey(newSub.productID)) {
            // remove the item from UI and internal storage
            ordersToShow.Remove(workLists[newSub.productID][0]);
            workLists[newSub.productID].Remove(workLists[newSub.productID][0]);

            // tell game manager it's done
            GameManager.instance.OrderComplete();

            // make the present disappear
            Destroy(newSub.gameObject);
        }
    }
}
