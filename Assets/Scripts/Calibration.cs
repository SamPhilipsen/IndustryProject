using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calibration : MonoBehaviour
{
    [SerializeField]
    private GameObject communicationManager;
    [SerializeField]
    private GameObject dropDownBox;

    [SerializeField]
    private GameObject horizontalSliders;
    [SerializeField]
    private GameObject verticalSliders;
    [SerializeField]
    private GameObject speedSliders;

    [SerializeField]
    private List<GameObject> horizontalSlidersList;
    [SerializeField]
    private List<GameObject> verticalSlidersList;
    [SerializeField]
    private List<GameObject> speedSlidersList;

    private void Update()
    {
        //ChangeSliders();
    }

    private void Awake()
    {
        Calibrate();
    }

    public void ChangeSliders()
    {            
        if (dropDownBox.GetComponent<TMP_Dropdown>().value == 0)
        {
            horizontalSliders.SetActive(true);

            verticalSliders.SetActive(false);
            speedSliders.SetActive(false);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 1)
        {
            verticalSliders.SetActive(true);

            horizontalSliders.SetActive(false);
            speedSliders.SetActive(false);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 2)
        {
            speedSliders.SetActive(true);

            verticalSliders.SetActive(false);
            horizontalSliders.SetActive(false);
        }
    }

    public void Calibrate()
    {
        if (dropDownBox.GetComponent<TMP_Dropdown>().value == 0)
        {
            float turnoverValue = horizontalSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = horizontalSlidersList[0].GetComponent<Slider>().value - turnoverValue;
            float tempMaxValue = horizontalSlidersList[2].GetComponent<Slider>().value - turnoverValue;
            GlobalPotValues.horizontalValues = new DifferentPotValues(tempMinValue, turnoverValue, tempMaxValue);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 1)
        {
            float turnoverValue = verticalSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = verticalSlidersList[0].GetComponent<Slider>().value - turnoverValue;
            float tempMaxValue = verticalSlidersList[2].GetComponent<Slider>().value - turnoverValue;
            GlobalPotValues.verticalValues = new DifferentPotValues(tempMinValue, turnoverValue, tempMaxValue);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 2)
        {
            float turnoverValue = speedSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = speedSlidersList[0].GetComponent<Slider>().value - turnoverValue;
            float tempMaxValue = speedSlidersList[2].GetComponent<Slider>().value - turnoverValue;
            GlobalPotValues.speedValues = new DifferentPotValues(tempMinValue, turnoverValue, tempMaxValue);
        }

        //different method
        //float i = 0;

        //if (tempPotValue > 0)
        //{
        //    i = tempPotValue / tempMaxValue;
        //}
        //else if (tempPotValue < 0)
        //{
        //    i = tempPotValue / tempMinValue;
        //}
        //else if (tempPotValue == 0)
        //{
        //    i = 0;
        //}

    }
}
