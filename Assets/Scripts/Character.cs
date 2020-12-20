using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Character : MonoBehaviour, ISelectable
{
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private Material[] materials;

    private Renderer visual = null;
    public Queue<Command> CommandQuque = new Queue<Command>();

    private bool isSelected;
    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            // Apply tint accordingly
            if (value)
            {
                visual.material = materials[1];
            }
            else
            {
                visual.material = materials[0];
            }
            isSelected = value;
        }
    }

    private void Start()
    {
        visual = gameObject.GetComponent<Renderer>();
        IsSelected = false;
    }
}
