using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Item : MonoBehaviour
{
    public float health = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (health < 0f)
        {
            Destroy(gameObject);
        }
    }
}
