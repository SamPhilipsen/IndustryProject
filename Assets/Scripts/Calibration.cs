using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calibration : MonoBehaviour
{
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

    private List<float> allValues = new List<float>();
    private List<string> playerPrefIndex = new List<string>();

    private void Awake()
    {
        int totalIndexes = horizontalSlidersList.Count + verticalSlidersList.Count + speedSlidersList.Count;
        for (int i = 0; i < totalIndexes; i++)
        {
            playerPrefIndex.Add(i.ToString());
        }

        allValues = PlayerPrefshandler.GetValues(playerPrefIndex);
        SetValues(allValues);

        PlayerPrefshandler.SetGlobalValues(playerPrefIndex);
    }

    public void ChangeSliders()
    {
        PlayerPrefs.Save();
        allValues = PlayerPrefshandler.GetValues(playerPrefIndex);

        SetValues(allValues);
        if (dropDownBox.GetComponent<TMP_Dropdown>().value == 0)
        {
            horizontalSliders.SetActive(true);
            foreach (GameObject obj in horizontalSlidersList)
            {
                obj.GetComponent<SliderValueToText>().ChangeText();
            }

            verticalSliders.SetActive(false);
            speedSliders.SetActive(false);            
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 1)
        {
            verticalSliders.SetActive(true);
            foreach (GameObject obj in verticalSlidersList)
            {
                obj.GetComponent<SliderValueToText>().ChangeText();
            }

            horizontalSliders.SetActive(false);
            speedSliders.SetActive(false);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 2)
        {
            speedSliders.SetActive(true);
            foreach (GameObject obj in speedSlidersList)
            {
                obj.GetComponent<SliderValueToText>().ChangeText();
            }

            verticalSliders.SetActive(false);
            horizontalSliders.SetActive(false);
        }
    }

    public void Calibrate()
    {
        //Check the turnoverValue
        if (dropDownBox.GetComponent<TMP_Dropdown>().value == 0)
        {
            float turnoverValue = horizontalSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = horizontalSlidersList[0].GetComponent<Slider>().value;
            float tempMaxValue = horizontalSlidersList[2].GetComponent<Slider>().value;
            
                GlobalPotValues.horizontalValues.minValue = tempMinValue;
                GlobalPotValues.horizontalValues.turnoverValue = turnoverValue;
                GlobalPotValues.horizontalValues.maxValue = tempMaxValue;
            
            PlayerPrefshandler.SaveValues(0, horizontalSlidersList, playerPrefIndex);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 1)
        {
            float turnoverValue = verticalSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = verticalSlidersList[0].GetComponent<Slider>().value;
            float tempMaxValue = verticalSlidersList[2].GetComponent<Slider>().value;
            
                GlobalPotValues.verticalValues.minValue = tempMinValue;
                GlobalPotValues.verticalValues.turnoverValue = turnoverValue;
                GlobalPotValues.verticalValues.maxValue = tempMaxValue;
            
            PlayerPrefshandler.SaveValues(1, verticalSlidersList, playerPrefIndex);
        }
        else if (dropDownBox.GetComponent<TMP_Dropdown>().value == 2)
        {
            float turnoverValue = speedSlidersList[1].GetComponent<Slider>().value;
            float tempMinValue = speedSlidersList[0].GetComponent<Slider>().value;
            float tempMaxValue = speedSlidersList[2].GetComponent<Slider>().value;
            
                GlobalPotValues.speedValues.minValue = tempMinValue;
                GlobalPotValues.speedValues.turnoverValue = turnoverValue;
                GlobalPotValues.speedValues.maxValue = tempMaxValue;
            
            PlayerPrefshandler.SaveValues(2, speedSlidersList, playerPrefIndex);
        }
        allValues = PlayerPrefshandler.GetValues(playerPrefIndex);
    }

    private void SetValues(List<float> values)
    {
                //Horizontal
                    horizontalSlidersList[0].GetComponent<Slider>().value = values[0];
                    horizontalSlidersList[1].GetComponent<Slider>().value = values[1];
                    horizontalSlidersList[2].GetComponent<Slider>().value = values[2];
                //Vertical
                    verticalSlidersList[0].GetComponent<Slider>().value = values[3];
                    verticalSlidersList[1].GetComponent<Slider>().value = values[4];
                    verticalSlidersList[2].GetComponent<Slider>().value = values[5];
                //Speed
                    speedSlidersList[0].GetComponent<Slider>().value = values[6];
                    speedSlidersList[1].GetComponent<Slider>().value = values[7];
                    speedSlidersList[2].GetComponent<Slider>().value = values[8];
    }    
}
