using System;
using System.IO.Ports;
using System.Management;
using UnityEngine;

namespace ArduinoControl
{
    class ArduinoController : MonoBehaviour
    {
        public int baudRate { get; private set; }
        private SerialPort serialPort { get; set; }
        private MessageCreator messageCreator;
        public string readMessage { get; private set; }
        public string[] MessageByPayload { get; private set; }

        public ArduinoController(int baudrate, string portname, MessageCreator messagecreator)
        {
            if (portname == null)
            {
                throw new ArgumentNullException("COM PORT");
            }
            if (baudrate < 9600)
            {
                throw new ArgumentOutOfRangeException("BaudRate");
            }
            if (messagecreator == null)
            {
                throw new ArgumentNullException("Message Builder");
            }

            baudRate = baudrate;
            messageCreator = messagecreator;
            serialPort = new SerialPort(portname); //Make sure this changes, otherwise things will HIT. THE. FAN.
        }

       
        public ArduinoController()
        {
            serialPort = new SerialPort(" ");
            messageCreator = new MessageCreator('#','@');
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
        public string CheckCOMPort(string valueToCheck)
        {
            ManagementScope managementScope = new ManagementScope();
            SelectQuery query = new SelectQuery("Select * from Win32_SerialPort");
            ManagementObjectSearcher managmentObjectSearcher = new ManagementObjectSearcher(managementScope,query);

            try
            {
                foreach (ManagementObject managementObject in managmentObjectSearcher.Get())
                {
                    string description = managementObject["Description"].ToString();
                    string deviceID = managementObject["DeviceID"].ToString();
                    if (description.Contains(valueToCheck))
                    {
                        return deviceID;
                    }
                }
            }
            catch (ManagementException exception)
            {
                MessageBox.Show(exception.Message);
            }

            return null;
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
    }
}
