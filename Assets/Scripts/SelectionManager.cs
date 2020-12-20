using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton RTS-style selection manager
/// </summary>
class SelectionManager : MonoBehaviour
{
    public ISelectable Selection
    {
        get; private set;
    }

    private void Update()
    {
        // LMB to select, as all RTS should have
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                UnSelectAll();
                ISelectable selectable = hit.collider.gameObject.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    selectable.IsSelected = true;
                    Selection = selectable;
                }
            }
        }
        // Without interaction, RMB would just issue a movement order
        // Refactor later
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && Selection is Character)
            {
                Character character = Selection as Character;
                character.CommandQueue.Enqueue(new MovementOrder(new Vector2(hit.point.x, hit.point.z), 0.01f));
                Debug.Log("Movement order issued");
            }
        }
    }

    private void UnSelectAll()
    {
        if (Selection != null)
        {
            Selection.IsSelected = false;
        }
        Selection = null;
    }
}
