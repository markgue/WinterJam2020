using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemId;

    public float health = 1.0f;

    public LayerMask playerLayer;

    // to show tooltip
    [SerializeField]
    private GameObject hoverToolTip;
    private GameObject hoverInstance = null;

    // parameters
    [SerializeField]
    private float hoverHeight;

    private bool isHovered = false;


    // Update is called once per frame
    void Update()
    {
        if (health < 0f)
        {
            Destroy(gameObject);
        }

        //// kick mechanism
        //Collider[] hits = Physics.OverlapSphere(transform.position, hitRadius, playerLayer);
        //if (hits.Length != 0) {
        //    // when there's hit: kick myself
        //    foreach (Collider hit in hits) {
        //        gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hit.transform.position).normalized * hitScale, ForceMode.Impulse);
        //    }
        //}

        if (!isHovered) {
            if (hoverInstance != null) {
                Destroy(hoverInstance);
                hoverInstance = null;
            }
        }
        else if (isHovered) {
            if (hoverInstance == null) {
                hoverInstance = Instantiate(hoverToolTip, transform.position + new Vector3(0, hoverHeight, 0), Quaternion.identity);
                hoverInstance.transform.parent = transform;
            }
            else {
                hoverInstance.transform.position = transform.position + new Vector3(0, hoverHeight, 0);
            }
            isHovered = false;
        }
    }

    
    // player message ///////////////////////////////////////////////////////
    virtual public void Take() {}
    virtual public void Put() {}
    
    virtual public void Hover() {
        isHovered = true;
    }
}
