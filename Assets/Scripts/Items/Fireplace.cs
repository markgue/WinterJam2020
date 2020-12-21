using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an always-on fireplace that destroys any item that is within its "destroy zone"
/// </summary>
class Fireplace : MonoBehaviour
{
    [SerializeField]
    private Collider zoneOfDestruction;
    public float DestroyTime = 30.0f;

    struct ItemOnFire
    {
        public ItemOnFire(Item item, float time)
        {
            this.item = item;
            remainingTime = time;
        }
        public Item item;
        public float remainingTime;
    }

    [SerializeField]
    private List<ItemOnFire> itemsOnFire = new List<ItemOnFire>();

    private void OnCollisionEnter(Collision collision)
    {
        Item target = collision.gameObject.GetComponent<Item>();
        if (target != null)
        {
            Debug.Log("Item is put in fire!");
            // Some visual effect would be nice
            bool isInList = false;
            foreach (ItemOnFire f in itemsOnFire)
            {
                if (target == f.item)
                {
                    isInList = true;
                }
            }
            if (!isInList)
            {
                ItemOnFire fire = new ItemOnFire(target, DestroyTime);
                itemsOnFire.Add(fire);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < itemsOnFire.Count; i++)
        {
            ItemOnFire fire = itemsOnFire[i];
            fire.remainingTime = fire.remainingTime - Time.unscaledDeltaTime;
            Debug.Log(fire.remainingTime);
            if (fire.remainingTime < 0f)
            {
                Debug.Log("Item destroyed in fire");
                Destroy(fire.item.gameObject);
                itemsOnFire.Remove(fire);
            }
        }
    }
}
