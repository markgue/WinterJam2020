using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float health = 1.0f;

    public LayerMask playerLayer;

    // parameters
    [SerializeField]
    private float hitRadius = 2;
    [SerializeField]
    private float hitScale = 10;

    // Update is called once per frame
    void Update()
    {
        if (health < 0f)
        {
            Destroy(gameObject);
        }

        // kick mechanism
        Collider[] hits = Physics.OverlapSphere(transform.position, hitRadius, playerLayer);
        if (hits.Length != 0) {
            // when there's hit: kick myself
            foreach (Collider hit in hits) {
                gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hit.transform.position) * hitScale, ForceMode.Impulse);
            }
        }

    }

    
    // player message ///////////////////////////////////////////////////////
    virtual public void Take() {}
    virtual public void Put() {}
}
