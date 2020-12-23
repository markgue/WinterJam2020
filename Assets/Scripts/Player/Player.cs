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
    //[SerializeField]
    //private float grabbingRange = 2.0f;
    public KeyCode PickAndThrowKey = KeyCode.E;
    public KeyCode PickAndThrowJoystick = KeyCode.E;
    [SerializeField]
    private Transform holdingPos;
    [SerializeField]
    private Transform leftHandPos;
    [SerializeField]
    private List<ListWrapper> itemToHandTransforms = new List<ListWrapper>();
    private Dictionary<string, Transform> itemToHandTransformsHash;
    [SerializeField]
    private float throwForce = 10.0f;
    private bool hasRecentlyThrown = false;

    [System.Serializable]
    public class ListWrapper
    {
        public string itemId;
        public Transform localTransformWhileHolding;
    }

    // new grab system
    [SerializeField]
    private float grabRadius;
    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private Vector2 tableGrabOffsetMagnitude;
    [SerializeField]
    private float tableGrabRadiusOffset;
    [SerializeField]
    private float tableGrabAngleDegreesThreshold;
    public LayerMask itemMask;
    public LayerMask detectionMask;

    public bool hasAxe = false;

    private void Awake()
    {
        itemToHandTransformsHash = new Dictionary<string, Transform>();
        foreach (ListWrapper listItem in itemToHandTransforms)
        {
            itemToHandTransformsHash[listItem.itemId] = listItem.localTransformWhileHolding;
        }
    }

    // Handles inputs in Update function once per frame
    private void Start() {
        GameManager.instance.playerObject = gameObject;
        holdingPos.SetParent(leftHandPos);
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

        //// checking holding animation
        //if (ItemHolding != null) {
        //    gameObject.GetComponent<Animator>().SetBool("isHoldingItem", true);
        //}
        //else {
        //    gameObject.GetComponent<Animator>().SetBool("isHoldingItem", false);
        //}

        controller.Move(move);

        // ground check and pushing player onto ground
        //gVelocity.y -= g * Time.deltaTime;
        //if (grounded && gVelocity.y <= 0)
        //{
            gVelocity.y = (grounded && gVelocity.y <= 0)? - 0.1f : gVelocity.y - g * Time.deltaTime;
        //}
        controller.Move(gVelocity * Time.deltaTime);

        // If we are near the table, we need to offset our collision for detecting items to account for difference in elevation and length
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        Collider[] nearTable = Physics.OverlapSphere(transform.position, collider.radius, detectionMask); // Good enough for detection purposes
        Vector3 grabOffset = Vector3.zero;
        float grabRadiusOffsetF = 0f;
        if (nearTable.Length > 0)
        {
            Collider tableCol = nearTable[0];
            Vector3 torwardsTableDirection = (tableCol.transform.position - transform.position).normalized;
            // Near table, use offseted collision method if we are facing the table
            if (Vector3.Dot(torwardsTableDirection, transform.forward.normalized) >= Mathf.Cos(Mathf.PI * tableGrabAngleDegreesThreshold / 180))
            {
                grabOffset = tableGrabOffsetMagnitude.x * transform.forward.normalized + tableGrabOffsetMagnitude.y * transform.up.normalized;
                grabRadiusOffsetF = tableGrabRadiusOffset;
            }
        }

        // tell items to show hover menu (not menu really but seems hover menu sounds more smooth :)
        Collider[] hitItems = Physics.OverlapSphere(grabPoint.position + grabOffset, grabRadius + grabRadiusOffsetF, itemMask);
        foreach (Collider it in hitItems)
        {
            // Debug.Log("Collision");
            it.gameObject.GetComponent<Item>().Hover();
        }

        AnimatorStateInfo info = GetComponent<Animator>().GetCurrentAnimatorStateInfo(GetComponent<Animator>().GetLayerIndex("Interaction"));
        if (info.IsName("Holding") && ItemHolding == null && !hasRecentlyThrown)
        {
            // Happens when the present is removed when walking into the chute
            GetComponent<Animator>().SetTrigger("Throw");
            hasRecentlyThrown = true;
        }

        // pickup / throw
        if (Input.GetKeyDown(PickAndThrowKey) || Input.GetKeyDown(PickAndThrowJoystick))
        {
            if (ItemHolding != null) {
                // throw current holding item towards the point
                Vector3 direction = transform.forward + Vector3.up * 0.15f;
                ItemHolding.transform.SetParent(null);
                ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                ItemHolding.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * throwForce);
                ItemHolding.gameObject.GetComponent<Item>().Put();
                ItemHolding = null;
                Debug.Log("item thrown");
                gameObject.GetComponent<Animator>().SetTrigger("Throw");
                hasRecentlyThrown = true;
            }
            else {
                if (hitItems.Length > 0) {
                    // pick the found item
                    Item target = hitItems[0].gameObject.GetComponent<Item>();
                    if (target != null && target.enabled) {
                        Transform localTransformForPlacement = itemToHandTransformsHash[target.itemId];
                        if (/*Vector3.Distance(target.transform.position, transform.position) <= grabbingRange*/ true) {
                            target.transform.position = holdingPos.position;
                            target.transform.SetParent(holdingPos);
                            if (localTransformForPlacement != null)
                            {
                                target.transform.localRotation = localTransformForPlacement.localRotation;
                                target.transform.localPosition = localTransformForPlacement.localPosition;
                            }

                            ItemHolding = target;
                            ItemHolding.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                            target.Take();
                            Debug.Log("Item picked up");
                            gameObject.GetComponent<Animator>().SetTrigger("Pickup");
                            hasRecentlyThrown = false;
                        }
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
    }
}
