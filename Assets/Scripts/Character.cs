using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ISelectable
{
    [SerializeField]
    private float movementSpeed = 1.0f;

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
            isSelected = value;
        }
    }


}
