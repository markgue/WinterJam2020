using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    private float progress = 100;
    private bool frozen = false;

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

    // control settings
    [SerializeField]
    private KeyCode chopKey = KeyCode.Q;
    [SerializeField]
    private float chopRadius = 1;
    [SerializeField]
    private float chopCD = 0;

    // part destruction
    [SerializeField]
    private GameObject[] parts;
    [SerializeField]
    private int[] chkPoints;


    // system messages //////////////////////////////////////////////
    private void Start() {
        frozen = false;
        progress = maxProgress;
    }

    private void Update() {
        if (frozen) return;
        
        float distanceFromPlayer = 0;

        // calculate x-z distance from player
        Vector3 diff = transform.position - GameManager.instance.playerObject.transform.position;
        Vector2 diff2 = new Vector2(diff.x, diff.z);
        distanceFromPlayer = diff2.magnitude;

        // if player has axe and player is close enough
        if (TreeManager.instance.playerHasAxe && distanceFromPlayer <= chopRadius) {
            UpdateTimer((timer + Time.deltaTime * slideSpeed) % maxTime);
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

                // update progress
                if (progress <= 0) {
                    // notify tree manager the tree is down
                    TreeManager.instance.TreeGotChopped(gameObject);
                    return;
                }
                else {
                    // animate the progress change
                    for (int i = 0; i < chkPoints.Length; i++) {
                        if (progress <= chkPoints[i]) {
                            parts[i].SetActive(false);
                        }
                    }

                    // freeze slide bar
                    StartCoroutine(ChopCoolDown(chopCD));
                }
            }
        }
        else {
            UpdateTimer(0);
            qteUI.SetActive(false);
        }
    }


    // timer management /////////////////////////////////////////////////
    private void UpdateTimer(float newTime) {
        timer = newTime;
        qteUI.GetComponent<ChopUI>().NewTime((newTime > 50)? 100 - newTime : newTime);
    }


    // coroutine ////////////////////////////////////////////////////////
    private IEnumerator ChopCoolDown(float coolDown) {
        frozen = true;
        
        float CD = coolDown;
        while (CD >= 0) {
            CD -= Time.deltaTime;
            yield return null;
        }

        frozen = false;
        UpdateTimer(0);
    }
}
