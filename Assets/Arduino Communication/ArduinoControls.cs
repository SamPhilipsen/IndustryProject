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
    private int baudRate = 9600;
    [SerializeField]
    char StartMarker = '@';
    [SerializeField]
    char EndMarker = '#';
    [SerializeField]
    char PayloadMarker = ':';
    private SerialPort serialPort { get; set; }
    private MessageCreator messageCreator;

    [SerializeField]
    public string[] Message;

    [SerializeField]
    public int Speed, HorizontalTilt, VerticalTilt;

    private string inboundMessage;

    public void Start()
    {
        messageCreator = new MessageCreator(StartMarker,EndMarker);
        string[] ports = GetPorts();
        serialPort = new SerialPort(ports[0]);
        serialPort.BaudRate = baudRate;
        Connect();

        if (serialPort.IsOpen == false)
        {
            //SEND HELP
        }
    }

    public void OnDestroy()
    {
        Disconnect();
    }

    private void Update()
    {
        if (ReadMessage())
        {
            ProcessMessage();
        }
    }

    enum CommandStructure
    {
        Identifier = 0,
        Payload = 1
    }

    private void ProcessMessage()
    {
        if (Int32.TryParse(Message[(int)CommandStructure.Payload], out int payload))
        {
            switch (Message[(int)CommandStructure.Identifier])
            {
                case "HRZ":
                    {
                        HorizontalTilt = payload;
                        break;
                    }
                case "VER":
                    {
                        VerticalTilt = payload;
                        break;
                    }
                case "SPD":
                    {
                        Speed = payload;
                        break;
                    }
                default:
                    break;
            }
        }
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
    public new bool SendMessage(string message)
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
            try
            {
                string readMessage = serialPort.ReadExisting();

                if (readMessage.Contains(EndMarker))
                {
                    inboundMessage += readMessage;
                    int endIndex = inboundMessage.IndexOf(EndMarker);
                    int startIndex = inboundMessage.IndexOf(StartMarker);

                    if (startIndex == -1 || endIndex == -1)
                    {
                        inboundMessage = "";
                        return false;
                    }

                    //Technically unsafe, but due to the order its FINE.
                    inboundMessage = inboundMessage.Remove(endIndex);
                    inboundMessage = inboundMessage.Remove(startIndex, 1);
                    inboundMessage = inboundMessage.Trim();
                    Message = inboundMessage.Split(PayloadMarker, StringSplitOptions.RemoveEmptyEntries);
                    inboundMessage = "";
                }
                else
                {
                    inboundMessage += readMessage;
                }
                return true;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                return false;
            }
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
