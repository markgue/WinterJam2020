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
    public KeyCode pickKey = KeyCode.F;
    public KeyCode cancelKey = KeyCode.E;

    // system messages ////////////////////////////////////////////////////////
    void Update()
    {
        // check actions!

        // pick/throw key
        // when there's item in front of the player and player is not holding anything: pick
        // when player is holding something: throw
        if (Input.GetKeyDown(pickKey)) {
            if (heldInHand) {
                // TODO: get the object by overlap sphere
                // TODO: send the item to PickUp()
                Debug.Log("take item");
            }
            else {
                Throw();
                Debug.Log("throw!");
            }
        }
        else if (Input.GetKeyDown(cancelKey)) {
            if (heldInHand) {
                Drop();
                Debug.Log("drop!");
            }
        }
    }


    // internal usages /////////////////////////////////////////////////////////
    // these are the actual functions taking place 
    private void PickUp(GameObject item) {
        // TODO: disable any physics checking on the item
        
        // hold the item on top of head
        item.transform.position = holdingPos.position;
        
        // take into inventory
        heldInHand = item;
    }

    private void Throw() {
        // only when something is in hand
        if (heldInHand != null) {
            // TODO: add a force onto the object
            heldInHand = null;
        }
    }

    private void Drop() {
        // i.e. cancel, drop the item in front of the player
    }
}
