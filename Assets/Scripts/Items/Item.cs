using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Item : MonoBehaviour
{
    public float health = 1.0f;

    public LayerMask playerLayer;

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

        // find player
        Collider[] hits = Physics.OverlapSphere(transform.position, hitRadius, playerLayer);
        if (hits.Length != 0) {
            // when there's hit: kick myself
            foreach (Collider hit in hits) {
                gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hit.transform.position) * hitScale, ForceMode.Impulse);
            }
        }

    }
}
