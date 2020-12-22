using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopUI : MonoBehaviour
{
    [SerializeField]
    private Gradient colorGradient;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image fill;

    //public float timer;

    //// Update is called once per frame
    //void Update()
    //{
    //    slider.value = timer;
    //    fill.color = colorGradient.Evaluate(timer / slider.maxValue);
    //}

    public void NewTime(float timer) {
        slider.value = timer;
        fill.color = colorGradient.Evaluate(slider.normalizedValue);
        Debug.Log(colorGradient.Evaluate(slider.normalizedValue));
    }
}
