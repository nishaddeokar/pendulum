using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Transform pivot;
    public Transform node;
    public Transform mass;
    public Slider slider1; //already a variable named slider in Slider_length.cs. Therefore, named slider1 to avoid confusion.

    public float maxDisplacement;
    public float currentDisplacement;
    public float lengthOfRod = 1f; //arbitrary value
    public float currentTime = 0f;
    public float timePeriod = 0f;
    public float gravitationalAcceleration = 9.80665f;
    public float maxAngle = 0f;
    public float currentAngle = 0f;
    public float dampingFactor = 0f;
    public float series;

    float FindLength(Transform mass, Transform pivot)
    {
        float length = Mathf.Sqrt(((mass.transform.position.x - pivot.transform.position.x) * (mass.transform.position.x - pivot.transform.position.x))
            + ((mass.transform.position.y - pivot.transform.position.y) * (mass.transform.position.y - pivot.transform.position.y))
            + ((mass.transform.position.z - pivot.transform.position.z) * (mass.transform.position.z - pivot.transform.position.z)));
        return length;
    }

    void CalculateConstants()
    {
        lengthOfRod = (FindLength(mass, pivot) - 5) / 10;
        maxDisplacement = maxAngle * lengthOfRod;
        series = (1 + (0.0625f * Mathf.Pow(maxAngle, 2)) + (0.0035807291667f * Mathf.Pow(maxAngle, 4)) + (0.00023464626736f * Mathf.Pow(maxAngle, 6)) + (0.000017356115674f * Mathf.Pow(maxAngle, 8)) + (0.0000013867625063f * Mathf.Pow(maxAngle, 10)));
        timePeriod = (2 * Mathf.PI) * Mathf.Sqrt(lengthOfRod / gravitationalAcceleration) *  series;
    }

    void SetNodePos()
    {
        node.gameObject.SetActive(true);
        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y); //the z-coordinate does not need to be defined, as it is defined separately in the subsequent line.
        mouse.z = 20f; //this is set to 20 as this is the distance the pivot (and therefore the mass) is from the camera.
        node.transform.position = Camera.main.ScreenToWorldPoint(mouse);
    }

    void Timer()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timePeriod)
        {
            currentTime -= timePeriod;
        }
    }

    void IfClicked()
    {
        SetNodePos();
        maxAngle = Mathf.Atan2((node.transform.position.y - pivot.transform.position.y), (node.transform.position.x - pivot.transform.position.x)) + (0.5f * Mathf.PI); //resultant angle is in radians.
        if (maxAngle > Mathf.PI) 
        { 
            maxAngle = maxAngle - (2 * Mathf.PI); 
        }
        pivot.transform.localEulerAngles = new Vector3(0, 0, maxAngle * Mathf.Rad2Deg); //Euler angles are in degrees. Therefore, maxAngle must be converted from radians to degrees.
        currentTime = 0;
    }

    void Motion()
    {
        CalculateConstants();
        currentDisplacement = maxDisplacement * Mathf.Cos((2 * Mathf.PI / timePeriod) * currentTime);
        currentAngle = currentDisplacement / lengthOfRod; //this angle is in radians.
        pivot.transform.localEulerAngles = new Vector3(0, 0, currentAngle * Mathf.Rad2Deg); //Euler angles are in degrees. Therefore, currentAngle must be converted from radians to degrees.
    }

    void Damping()
    {
        dampingFactor = slider1.value;
        if (dampingFactor == 1)
        {
            dampingFactor = 0.9999f;
        }
        dampingFactor = 1 - dampingFactor;
        maxAngle = maxAngle * Mathf.Exp(((Mathf.Log(dampingFactor)) / timePeriod) * Time.deltaTime);
        if (Mathf.Abs(maxAngle) < 0.0001f) //absolute used to account for angles in anticlockwise direction
        {
            maxAngle = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxAngle = 0f; //initial angle
        pivot.transform.localEulerAngles = new Vector3(0, 0, 0);
        currentTime = 0;
        CalculateConstants();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateConstants();
        node.gameObject.SetActive(false);
        Timer();
        Damping();
        if (Input.GetKey(KeyCode.Mouse0) && (Input.mousePosition.x > 200 || Input.mousePosition.y > 120)) //KeyCode.Mouse0 refers to the left/ primary mouse button.
        {
            IfClicked();
        }
        else
        {
            Motion();
        }
    }
}
