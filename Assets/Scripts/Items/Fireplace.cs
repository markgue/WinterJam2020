using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an always-on fireplace that destroys any item that is within its "destroy zone"
/// </summary>
class Fireplace : MonoBehaviour
{
    public float DPS = 10f;

    [SerializeField]
    private List<Item> itemsOnFire = new List<Item>();

    private void OnCollisionEnter(Collision collision)
    {
        Item target = collision.gameObject.GetComponent<Item>();
        if (target != null)
        {
            Debug.Log("Item is put in fire!");
            // Some visual effect would be nice
            if (!itemsOnFire.Contains(target))
            {
                itemsOnFire.Add(target);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < itemsOnFire.Count; i++)
        {
            Item item = itemsOnFire[i];
            item.health -= Time.deltaTime * DPS;
            if (item.health < 0)
            {
                itemsOnFire.RemoveAt(i);
            }
            Debug.Log(item.health);
        }
    }
}
