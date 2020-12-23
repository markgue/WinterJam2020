using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingQTE : MonoBehaviour
{
    private bool isPlayerWithinRange;
    private bool qteIsActive;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private ChopUI qteUI;  // Reuse chopUI
    [SerializeField]
    private KeyCode QTEKey = KeyCode.Q;
    [SerializeField]
    private int qteDrainRate = 50;  // Per second
    [SerializeField]
    private int qteGainRate = 5;  // Per Press

    private float qteHealth;
    private bool isQTEKeyDown;
    private bool hasWon;
    private bool finishTrigger;

    // Start is called before the first frame update
    void Awake()
    {
        isPlayerWithinRange = false;
        isQTEKeyDown = false;
        hasWon = false;
        finishTrigger = false;
        ResetQTE();
    }

    // Update is called once per frame
    void Update()
    {
        if (!qteIsActive && isPlayerWithinRange && Input.GetKeyDown(QTEKey))
        {
            qteIsActive = true;
            qteHealth = 30; // Out of 100
        }
        // TODO: Maybe add a cooldown?
        if (qteIsActive)
        {
            if (qteHealth <= 0f)
            {
                hasWon = false;
                ResetQTE();
                finishTrigger = true;
            }
            else if (qteHealth >= 100f)
            {
                hasWon = true;
                ResetQTE();
                finishTrigger = true;
            }

            if (isPlayerWithinRange && !isQTEKeyDown && Input.GetKeyDown(QTEKey))
            {
                isQTEKeyDown = true;
                qteHealth += qteGainRate;
            } else if (isQTEKeyDown && !Input.GetKeyDown(QTEKey))
            {
                isQTEKeyDown = false;
            } else
            {
                qteHealth -= qteDrainRate * Time.deltaTime;
            }
        }

        // Update qteUI
        if (qteUI.enabled)
        {
            qteUI.NewTime(qteHealth);
        }
    }

    public bool isQteDone(out bool win)
    {
        if (qteIsActive || !finishTrigger)
        {
            win = false;
            return false;
        }
        finishTrigger = false;
        if (hasWon)
        {
            win = true;
            return true;
        } else
        {
            win = false;
            return true;
        }
    }

    private void ResetQTE()
    {
        qteIsActive = false;
        qteHealth = 0;
        slider.value = 0f;
        qteUI.gameObject.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Update the compare tag to another comparison
            isPlayerWithinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Update the compare tag to another comparison
            isPlayerWithinRange = false;
        }
    }
}
