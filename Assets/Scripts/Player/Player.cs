using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : MonoBehaviour {
    public CharacterController controller;
    public float speed = 12f;
    Vector3 gVelocity;
    public float g = 9.8f;

    public float groundBuffer = 0.4f;
    public LayerMask groundMask;

    [SerializeField]
    private Transform groundChecker;
    private bool grounded;
    public Vector3 myDirection = new Vector3(1, 0, 0);

    public Item ItemHolding = null;
    [SerializeField]
    private float grabbingRange = 2.0f;
    public KeyCode PickAndThrowKey = KeyCode.E;
    public KeyCode PickAndThrowJoystick = KeyCode.E;
    [SerializeField]
    private Transform holdingPos;
    [SerializeField]
    private float throwForce = 10.0f;

    // new grab system
    [SerializeField]
    private float grabRadius;
    [SerializeField]
    private Transform grabPoint;
    public LayerMask itemMask;

    public bool hasAxe = false;

    // Handles inputs in Update function once per frame
    private void Start() {
        GameManager.instance.playerObject = gameObject;
    }

    void Update() 
    {
        grounded = Physics.CheckSphere(groundChecker.position, groundBuffer, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);
        move *= speed * Time.deltaTime;

        // rotate the player and check running animation
        if (move != Vector3.zero) {
            Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = rotation;
            gameObject.GetComponent<Animator>().SetBool("isRunning", true);
        }
        else {
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }

        controller.Move(move);

        // ground check and pushing player onto ground
        //gVelocity.y -= g * Time.deltaTime;
        //if (grounded && gVelocity.y <= 0)
        //{
            gVelocity.y = (grounded && gVelocity.y <= 0)? - 0.1f : gVelocity.y - g * Time.deltaTime;
        //}
        controller.Move(gVelocity * Time.deltaTime);

        // tell items to show hover menu (not menu really but seems hover menu sounds more smooth :)
        Collider[] hitItems = Physics.OverlapSphere(grabPoint.position, grabRadius, itemMask);
        foreach (Collider it in hitItems) {
            it.gameObject.GetComponent<Item>().Hover();
        }

        // pickup / throw
        if (Input.GetKeyDown(PickAndThrowKey) || Input.GetKeyDown(PickAndThrowJoystick))
        {
            if (ItemHolding != null) {
                // throw current holding item towards the point
                Vector3 direction = transform.forward;
                ItemHolding.transform.SetParent(null);
                ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                ItemHolding.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce);
                ItemHolding.gameObject.GetComponent<Item>().Put();
                ItemHolding = null;
                Debug.Log("item thrown");
                gameObject.GetComponent<Animator>().SetTrigger("Throw");
            }
            else {
                if (hitItems.Length > 0) {
                    // pick the found item
                    Item target = hitItems[0].gameObject.GetComponent<Item>();
                    if (target != null) {
                        if (Vector3.Distance(target.transform.position, transform.position) <= grabbingRange) {
                            target.transform.position = holdingPos.position;
                            target.transform.SetParent(holdingPos);
                            ItemHolding = target;
                            ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                            target.Take();
                            Debug.Log("Item picked up");
                        }
                        gameObject.GetComponent<Animator>().SetTrigger("Pickup");
                    }
                }
            }

            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    if (ItemHolding == null)
            //    {
            //        Item target = hit.collider.gameObject.GetComponent<Item>();
            //        if (target != null)
            //        {
            //            if (Vector3.Distance(target.transform.position, transform.position) <= grabbingRange)
            //            {
            //                target.transform.position = holdingPos.position;
            //                target.transform.SetParent(holdingPos);
            //                ItemHolding = target;
            //                ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //                target.Take();
            //                Debug.Log("Item picked up");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // throw current holding item towards the point
            //        Vector3 direction = hit.point - holdingPos.position;
            //        ItemHolding.transform.SetParent(null);
            //        ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //        ItemHolding.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce);
            //        ItemHolding.gameObject.GetComponent<Item>().Put();
            //        ItemHolding = null;
            //        Debug.Log("item thrown");
            //    }
            //}
        }

        // checking holding animation
        if (ItemHolding != null) {
            gameObject.GetComponent<Animator>().SetBool("isHolding", true);
        }
        else {
            gameObject.GetComponent<Animator>().SetBool("isHolding", false);
        }
    }
}
