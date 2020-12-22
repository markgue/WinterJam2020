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
    [SerializeField]
    private Transform holdingPos;
    [SerializeField]
    private float throwForce = 10.0f;

    public bool hasAxe = false;

    // Handles inputs in Update function once per frame
    private void Start() {
        GameManager.instance.playerObject = gameObject;
    }

    void Update() 
    {
        // this game won't need ground checking.
        grounded = Physics.CheckSphere(groundChecker.position, groundBuffer, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed * Time.deltaTime;

        controller.Move(move);

        gVelocity.y -= g * Time.deltaTime;

        if (grounded && gVelocity.y <= 0)
        {
            gVelocity.y = -0.1f;
        }

        controller.Move(gVelocity * Time.deltaTime);

        if (Input.GetKeyDown(PickAndThrowKey))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (ItemHolding == null)
                {
                    Item target = hit.collider.gameObject.GetComponent<Item>();
                    if (target != null)
                    {
                        if (Vector3.Distance(target.transform.position, transform.position) <= grabbingRange)
                        {
                            target.transform.position = holdingPos.position;
                            target.transform.SetParent(holdingPos);
                            ItemHolding = target;
                            ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                            target.Take();
                            Debug.Log("Item picked up");
                        }
                    }
                }
                else
                {
                    // throw current holding item towards the point
                    Vector3 direction = hit.point - holdingPos.position;
                    ItemHolding.transform.SetParent(null);
                    ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    ItemHolding.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce);
                    ItemHolding.gameObject.GetComponent<Item>().Put();
                    ItemHolding = null;
                    Debug.Log("item thrown");
                }
            }
        }
    }
}
