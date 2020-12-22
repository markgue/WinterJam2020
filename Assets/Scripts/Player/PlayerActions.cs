using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    /// <summary>
    /// I'm imagining all the player can do is "press E to ..."
    ///     1. pickup
    ///     2. throw
    ///     3. interact
    /// pick up will probably be a separated action from interact, since 
    /// </summary>

    // reference points ///////////////////////////////////////////////////////
    public Transform holdingPos;
    
    // "inventory" ////////////////////////////////////////////////////////////
    private GameObject heldInHand = null; // use a gameobject for now

    // settings ///////////////////////////////////////////////////////////////
    [SerializeField]
    private KeyCode chopKey = KeyCode.Q;


    // system messages ////////////////////////////////////////////////////////
    void Update()
    {
        // qte actions?

    }
}
