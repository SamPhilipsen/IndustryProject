using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Management;


public class AutoDetectArduino : MonoBehaviour
{
    [SerializeField] string[] comPorts;

    private void Start()
    {
        AutodetectArduinoPort();
    }

    private string[] AutodetectArduinoPort()
    {
        comPorts = SerialPort.GetPortNames();
        return comPorts;
    }
}
