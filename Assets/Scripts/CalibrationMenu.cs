using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject calibrationMenu;

    private bool menuActive = false;
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (menuActive == false)
            {
                calibrationMenu.SetActive(true);
                menuActive = true;
            }
            else if (menuActive == true)
            {
                calibrationMenu.SetActive(false);
                menuActive = false;
            }
        }
    }
}
