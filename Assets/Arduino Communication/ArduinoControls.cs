using ArduinoControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ArduinoControls : MonoBehaviour
{
    [SerializeField]
    public int baudRate { get; private set; }
    [SerializeField]
    char StartMarker;
    [SerializeField]
    char EndMarker;
    private SerialPort serialPort { get; set; }
    private MessageCreator messageCreator;
    public string readMessage { get; private set; }
    public string[] MessageByPayload { get; private set; }

    public void Start()
    {
        messageCreator = new MessageCreator(StartMarker,EndMarker);
    }

    /// <summary>
    /// Connects to the serial port.
    /// </summary>
    public void Connect()
    {
        if (serialPort.IsOpen == false)
        {
            serialPort.Open();
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }
    }

    /// <summary>
    /// Disconnects from the serial port.
    /// </summary>
    public void Disconnect()
    {
        if (string.IsNullOrEmpty(serialPort.PortName) || string.IsNullOrWhiteSpace(serialPort.PortName))
        {
            return;
        }
        else if (serialPort.IsOpen == true)
        {
            serialPort.Close();
        }
    }

    /// <summary>
    /// Sends a message via the serial port to the arduino.
    /// </summary>
    /// <param name="message">The message that will be sent.</param>
    /// <returns>Whether it was successful.</returns>
    public bool SendMessage(string message)
    {
        if (serialPort.IsOpen == true)
        {
            messageCreator.Add(message);
            serialPort.Write(message);
            return true;
        }
        messageCreator.Reset();
        return false;
    }

    /// <summary>
    /// Reads a message from the serial port.
    /// </summary>
    /// <returns>Whether it was successful.</returns>
    public bool ReadMessage()
    {
        if (serialPort.IsOpen == true && serialPort.BytesToRead > 0)
        {
            string readMessage = serialPort.ReadExisting();
            if (string.IsNullOrEmpty(readMessage) || string.IsNullOrWhiteSpace(readMessage)) return false;

            readMessage = readMessage.Remove(readMessage.IndexOf(messageCreator.EndMarker));
            readMessage = readMessage.Substring(1);
            MessageByPayload = readMessage.Split(messageCreator.PayloadMarker);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the list of ports available to the program.
    /// </summary>
    /// <returns>An array of strings, which contains the available ports.</returns>
    public string[] GetPorts()
    {
        return SerialPort.GetPortNames();
    }

    /// <summary>
    /// A method to check if the COM device's description contains the desired value.
    /// </summary>
    /// <param name="valueToCheck">What to check for in the device description.</param>
    /// <returns>The COM port containing the right value, or null if none are found.</returns>
    //public string CheckCOMPort(string valueToCheck)
    //{
    //    ManagementScope managementScope = new ManagementScope();
    //    SelectQuery query = new SelectQuery("Select * from Win32_SerialPort");
    //    ManagementObjectSearcher managmentObjectSearcher = new ManagementObjectSearcher(managementScope, query);

    //    try
    //    {
    //        foreach (ManagementObject managementObject in managmentObjectSearcher.Get())
    //        {
    //            string description = managementObject["Description"].ToString();
    //            string deviceID = managementObject["DeviceID"].ToString();
    //            if (description.Contains(valueToCheck))
    //            {
    //                return deviceID;
    //            }
    //        }
    //    }
    //    catch (ManagementException exception)
    //    {
    //        MessageBox.Show(exception.Message);
    //    }

    //    return null;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="portName">The new port.</param>
    /// <returns>True is successful, false if failed.</returns>
    public bool ChangePort(string portName)
    {
        if (string.IsNullOrEmpty(portName))
        {
            return false;
        }
        serialPort.PortName = portName;
        return true;
    }
}
