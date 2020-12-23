using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField]
    private float maxOffset = 1f;
    [SerializeField]
    private float movementPeriodSeconds = 1f; 

    private float startTime;
    private Vector3 initialPos;
    private RectTransform rectTransform;

    private void Start()
    {
        startTime = Time.time;
        rectTransform = GetComponent<RectTransform>();
        initialPos = rectTransform.position;
    }

    void Update()
    {
        float timeDiff = Time.time - startTime;
        float yPos = maxOffset * Mathf.Sin(2 * Mathf.PI / movementPeriodSeconds * timeDiff);
        rectTransform.position = initialPos + Vector3.up * yPos;
    }
}
