using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    private float progress = 100;
    private bool activated = false;

    // progress tracker
    [SerializeField]
    private int highDamage, lowDamage, maxProgress;

    // QTE
    private float timer;
    [SerializeField]
    private float maxTime, critIntervalL, critIntervalH, normIntervalL, normIntervalH;
    [SerializeField]
    private float slideSpeed = 1;
    [SerializeField]
    private GameObject qteUI;

    // settings
    [SerializeField]
    private KeyCode chopKey = KeyCode.Q;
    [SerializeField]
    private float chopRadius = 1;

    // part destruction
    [SerializeField]
    private GameObject[] parts;
    [SerializeField]
    private int[] chkPoints;


    // system messages //////////////////////////////////////////////
    private void Start() {
        activated = false;
        progress = maxProgress;
    }

    private void Update() {
        float distanceFromPlayer = 0;

        // calculate x-z distance from player
        Vector3 diff = transform.position - GameManager.instance.playerObject.transform.position;
        Vector2 diff2 = new Vector2(diff.x, diff.z);
        distanceFromPlayer = diff2.magnitude;

        // if player has axe and player is close enough
        if (TreeManager.instance.playerHasAxe && distanceFromPlayer <= chopRadius) {
            timer = (timer + Time.deltaTime * slideSpeed) % maxTime;

            // update UI
            qteUI.GetComponent<ChopUI>().NewTime((timer > 50)? 100-timer : timer);
            qteUI.SetActive(true);

            // player chops
            if (Input.GetKeyDown(chopKey)) {
                int rawDamage = 0;
                
                // check time interval
                if (timer > critIntervalL && timer < critIntervalH) {
                    rawDamage = highDamage;
                }
                else if (timer > normIntervalL && timer < normIntervalH) {
                    rawDamage = lowDamage;
                }
                else {
                    rawDamage = 0;
                }

                progress -= rawDamage;

                timer = 0;
            }

            // update progress
            if (progress <= 0) {
                // notify tree manager the tree is down
                TreeManager.instance.TreeGotChopped(gameObject);
            }
            else {
                // notify animator current progress
                for (int i = 0; i < chkPoints.Length; i++) {
                    if (progress <= chkPoints[i]) {
                        parts[i].SetActive(false);
                    }
                }
            }
        }
        else {
            timer = 0;
            qteUI.GetComponent<ChopUI>().NewTime(timer);
            qteUI.SetActive(false);
        }
    }
}
