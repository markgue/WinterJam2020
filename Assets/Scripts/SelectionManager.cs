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
                    Debug.Log("Selected");
                    selectable.IsSelected = true;
                    Selection = selectable;
                }
            }
        }
        // Without interaction, RMB would just issue a movement order
        if (Input.GetMouseButtonDown(1))
        {

        }
    }

    private void UnSelectAll()
    {
        if (Selection != null)
        {
            Debug.Log("Unselect all");
            Selection.IsSelected = false;
        }
        Selection = null;
    }
}
