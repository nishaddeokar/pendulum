using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_length : MonoBehaviour
{
    public Transform rod;
    public Transform mass1; //already a variable named mass in Main.cs. Therefore, named mass1 to avoid confusion.
    public Slider slider;
    public float length;

    // Update is called once per frame
    void Update()
    {
        length = slider.value;
        rod.localScale = new Vector3(rod.localScale.x, length, rod.localScale.z);
        rod.localPosition = new Vector3(rod.localPosition.x, -(length / 2), rod.localPosition.z); //as y-position = -(y-scale / 2)
        mass1.localScale = new Vector3(mass1.localScale.x, 1 / length, mass1.localScale.z); //as scale of mass = (1 / scale of rod)
    }
}
