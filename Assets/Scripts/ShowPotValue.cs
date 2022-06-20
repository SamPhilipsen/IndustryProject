using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowPotValue : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [Range(0, 2)]
    public int Type;

    [SerializeField]
    private GameObject communicationManager;
    [SerializeField]
    private string arduinoEnabled;

    private void Update()
    {
        if (communicationManager.GetComponent<Unity_recive_data_from_Arduino>() == true)
        {
            if (communicationManager.GetComponent<Unity_recive_data_from_Arduino>().isConnected)
            {
                SetText();
            }            
        }        
    }

    private void SetText()
    {
        switch (Type)
        {
            case 0://Horizontal
                text.text = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().direction.ToString();
                break;
            case 1://Vertical
                text.text = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().height.ToString();
                break;
            case 2://Speed
                text.text = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().speed.ToString();
                break;
            default:
                break;
        }
    }
}
