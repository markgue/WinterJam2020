using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    private List<Item> inputs = new List<Item>();

    [System.Serializable]
    public class ListWrapper
    {
        public string itemId;
        public List<string> craftingMaterialsItemIds;
    }

    [SerializeField]
    private List<ListWrapper> itemToCraftingMaterials;
    [SerializeField]
    private Dictionary<string, List<string>> itemToCraftingMaterialsHash;
    private Dictionary<string, string> craftingMaterialToItemHash;

    private bool craftingTableActive;
    private string itemToCraftOnceActive;

    // Start is called before the first frame update
    void Awake()
    {
        // Create hashes for quick access of items and crafting materials
        itemToCraftingMaterialsHash = new Dictionary<string, List<string>>();
        craftingMaterialToItemHash = new Dictionary<string, string>();
        for (int i = 0; i < itemToCraftingMaterials.Count; i++)
        {
            ListWrapper item = itemToCraftingMaterials[i];
            itemToCraftingMaterialsHash[item.itemId] = new List<string>();
            List<string> craftingMats = item.craftingMaterialsItemIds;
            for (int j = 0; j < craftingMats.Count; j++)
            {
                itemToCraftingMaterialsHash[item.itemId].Add(craftingMats[j]);
                craftingMaterialToItemHash[craftingMats[j]] = item.itemId;
            }
        }
        craftingTableActive = false;
        itemToCraftOnceActive = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (craftingTableActive)
        {
            // TODO: Signal crafting QTE
            Debug.Log("Table Active with item: " + itemToCraftOnceActive);
        } else
        {
            if (inputs.Count > 0)
            {
                // Check if inputs form a complete item
                string currentItemId = "";
                List<string> materials = new List<string>();
                int inputCounter = 0;
                foreach (Item item in inputs)
                {
                    inputCounter += 1;
                    Debug.Log("Item id: " + item.itemId);
                    if (currentItemId.Length == 0)
                    {
                        if (!craftingMaterialToItemHash.TryGetValue(item.itemId, out currentItemId))
                        {
                            break;
                        }
                        materials = new List<string>(itemToCraftingMaterialsHash[currentItemId]);
                    }
                    if (!materials.Remove(item.itemId))
                    {
                        // No matching materials or have extra materials
                        break;
                    }
                    if (materials.Count == 0 && inputCounter == inputs.Count)
                    {
                        // Matching item
                        craftingTableActive = true;
                        itemToCraftOnceActive = currentItemId;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Item target = other.GetComponent<Item>();
        Rigidbody rb = other.GetComponent<Rigidbody>();
        // Ignore if the unique item already exists in the inputs list (reference)
        if (target != null && inputs.FindIndex(x => target == x) == -1)
        {
            inputs.Add(target);
            if (rb != null)
            {
                // Assume all item the bench uses would have rigid bodies
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item target = other.GetComponent<Item>();
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (target != null)
        {
            inputs.Remove(target);
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    IEnumerator FlyTowardsPosition()
    {
        return null;
    }
}
