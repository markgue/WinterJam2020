using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{
    public override void Take() {
        TreeManager.instance.playerHasAxe = true;
    }

    public override void Put() {
        TreeManager.instance.playerHasAxe = false;
    }
}
