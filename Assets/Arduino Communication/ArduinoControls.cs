using ArduinoControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoControls : MonoBehaviour, IArduinoData
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
    private string[] Message;

    public Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
    int HorizontalTilt, VerticalTilt, Rotations;

    private string inboundMessage;

    public int Roll { get { return keyValuePairs["HRZ"]; } }

    public int Pitch { get { return keyValuePairs["VER"]; } }

    public int Speed { get { return keyValuePairs["SPD"]; } }

    public void Start()
    {
        string[] ports = GetPorts();
        serialPort = new SerialPort(ports[0]);
        messageCreator = new MessageCreator(StartMarker,EndMarker);
        serialPort.BaudRate = baudRate;

        PrepareCommands();
        Connect();

        if (serialPort.IsOpen == false)
        {
            throw new Exception("Hardware could not connect!");
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

    private void PrepareCommands()
    {
        keyValuePairs.Add("HRZ", HorizontalTilt);
        keyValuePairs.Add("VER", VerticalTilt);
        keyValuePairs.Add("SPD", Rotations);
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
            keyValuePairs[Message[(int)CommandStructure.Identifier]] = payload;
        }
    }   


    /// <summary>
    /// Connects to the serial port.
    /// </summary>
    public void Connect()
    {
        if (serialPort.IsOpen == false)
        {
            string[] ports = GetPorts();
            serialPort = new SerialPort(ports[0]);

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
            string readMessage = serialPort.ReadExisting();

            try
            {
                if (readMessage.Contains(EndMarker))
                {
                    inboundMessage += readMessage;
                    int endIndex = inboundMessage.IndexOf(EndMarker);
                    int startIndex = inboundMessage.IndexOf(StartMarker);

                    if (startIndex == -1 || endIndex == -1 || startIndex == inboundMessage.Length - 1)
                    {
                        inboundMessage = "";
                        return false;
                    }

                    //Technically unsafe, but due to the order its FINE.
                    inboundMessage = inboundMessage.Remove(endIndex);

                    if (1 > inboundMessage.Length - startIndex)
                    {
                        return false;
                    }

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

    public bool isConnected()
    {
        if (serialPort == null) return false;
        else return serialPort.IsOpen;
    }
}
