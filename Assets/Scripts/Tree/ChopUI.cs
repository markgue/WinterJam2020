using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopUI : MonoBehaviour
{
    [SerializeField]
    private Gradient colorGradient;

    [SerializeField]
    private GameObject slider, fill;

    public float timer;

    // Update is called once per frame
    void Update()
    {
        slider.GetComponent<Slider>().value = timer;
        fill.GetComponent<Image>().color = colorGradient.Evaluate(timer / slider.GetComponent<Slider>().maxValue);
    }
}
