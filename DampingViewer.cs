using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DampingViewer : MonoBehaviour
{
    public Text sliderValue;
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        sliderValue.text = "Damping: " + slider.value.ToString("0.000");
    }
}