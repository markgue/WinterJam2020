using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : MonoBehaviour
{
    [SerializeField]
    private Image craftableIconImage;
    [SerializeField]
    private Transform craftCreationPoint;
    [SerializeField]
    private CraftingQTE craftingQte;
    [SerializeField]
    private Vector3 disperseDirection = new Vector3(-1f, 1f, 0f);
    [SerializeField]
    private float disperseStrength = 1.0f;
    [SerializeField]
    private float pullForceStrength = 1.0f;
    [SerializeField]
    private float pullForceRadiusStop = 1.0f;

    private List<Item> inputs = new List<Item>();

    [System.Serializable]
    public class ListWrapper
    {
        public string itemId;
        public Sprite icon;
        public List<string> craftingMaterialsItemIds;
    }

    [SerializeField]
    private List<ListWrapper> itemToCraftingMaterials;
    [SerializeField]
    private Dictionary<string, List<string>> itemToCraftingMaterialsHash;
    private Dictionary<string, Sprite> itemToCraftingIconHash;
    private Dictionary<string, string> craftingMaterialToItemHash;

    private bool craftingTableActive;
    private string itemToCraftOnceActive;
    private List<Item> activeCraftingInputs;
    private BoxCollider stickyTrigger;

    // Start is called before the first frame update
    void Awake()
    {
        // Create hashes for quick access of items and crafting materials
        itemToCraftingMaterialsHash = new Dictionary<string, List<string>>();
        craftingMaterialToItemHash = new Dictionary<string, string>();
        itemToCraftingIconHash = new Dictionary<string, Sprite>();
        for (int i = 0; i < itemToCraftingMaterials.Count; i++)
        {
            ListWrapper item = itemToCraftingMaterials[i];
            itemToCraftingIconHash[item.itemId] = item.icon;
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
        craftableIconImage.enabled = false;
        craftingQte.gameObject.SetActive(false);
        stickyTrigger = GetComponent<BoxCollider>();
    }

    // Todo: Handle case where more items are thrown after the proper materials are there
    void Update()
    {
        if (craftingTableActive)
        {
            craftableIconImage.sprite = itemToCraftingIconHash[itemToCraftOnceActive];
            craftableIconImage.enabled = true;
            craftingQte.gameObject.SetActive(true);
            bool success = false;
            // Suck the materials torwards the creation point while the table is active
            foreach(Item item in activeCraftingInputs)
            {
                Rigidbody itemRB = item.GetComponent<Rigidbody>();
                itemRB.isKinematic = false;
                Vector3 forceDirectionRaw = craftCreationPoint.position - item.transform.position;
                if (forceDirectionRaw.magnitude >= pullForceRadiusStop)
                {
                    Vector3 forceDirection = forceDirectionRaw.normalized;
                    itemRB.AddForce(forceDirection * pullForceStrength);
                }
            }

            if (craftingQte.isQteDone(out success))
            {
                if (success) {
                    // Return correct item
                    Debug.Log("Success");
                    foreach (Item item in activeCraftingInputs)
                    {
                        inputs.Remove(item);
                        Destroy(item.gameObject);
                    }
                } else
                {
                    // Disperse items
                    Debug.Log("Failure");
                    foreach (Item item in activeCraftingInputs)
                    {
                        Rigidbody rb = item.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.velocity = disperseDirection * disperseStrength;
                        StartCoroutine(stickyTableCooldown(2f));
                        inputs.Remove(item);
                    }
                }
                craftingQte.gameObject.SetActive(false);
                craftingTableActive = false;
            }
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
                        activeCraftingInputs = new List<Item>(inputs);
                        stickyTrigger.enabled = false;
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
        if (!craftingTableActive && target != null && inputs.FindIndex(x => target == x) == -1)
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
        }
    }

    private IEnumerator stickyTableCooldown(float seconds)
    {
        stickyTrigger.enabled = false;
        yield return new WaitForSeconds(seconds);
        stickyTrigger.enabled = true;
    }
}
