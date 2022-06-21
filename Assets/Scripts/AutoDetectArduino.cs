using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;


public class AutoDetectArduino : MonoBehaviour
{
    [SerializeField] string arduinoCom;

    private void Start()
    {
        AutodetectArduinoPort();
    }

    private void AutodetectArduinoPort()
    {
        string[] comPorts = SerialPort.GetPortNames();
        arduinoCom = comPorts[0]; 
    }
}
