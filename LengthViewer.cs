using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LengthViewer : MonoBehaviour
{
    public Text sliderValue;
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        sliderValue.text = "Length: " + ((slider.value - 5) / 10).ToString("0.000");
    }
}
