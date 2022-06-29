using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Threading;
using System.Windows;
using System.Linq;

public class Unity_recive_data_from_Arduino : MonoBehaviour
{
    [Header("ComPort")]
    public string com_port_number;
    private SerialPort serialPort;
    private Thread receiveThread;

    [Header("DolphinValues")]
    [SerializeField] public int speed;
    [SerializeField] public int direction;
    [SerializeField] public int height;

    public bool isConnected = false;

    void Start()
    {
        init_serial();

        //StartCoroutine(this.ReceiveThread());
    }

    void FixedUpdate()
    {
        if (Time.frameCount % 120 == 0)
        {
            System.GC.Collect();
        }
    }

    void OnDestroy()
    {
        if (isConnected)
        {
            if (receiveThread != null)
                receiveThread.Abort();
            if (serialPort.IsOpen)
                serialPort.Close();
        }        
    }
    private void init_serial()
    {
        var isValid = SerialPort.GetPortNames().Any(x => string.Compare(x, com_port_number, true) == 0);

        if (!isValid)
        {
            isConnected = false;
            return;
            //throw new System.IO.IOException(string.Format("{0} port was not found", com_port_number));
        }

        isConnected = true;

        try
        {
            serialPort = new SerialPort(com_port_number);

            serialPort.BaudRate = 9600;

            serialPort.Encoding = Encoding.ASCII;

            serialPort.StopBits = StopBits.One;

            serialPort.DataBits = 8;

            serialPort.Parity = Parity.None;

            serialPort.Open();

            receiveThread = Loom.RunAsync(() =>
            {
                while (true)
                {
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        try
                        {

                            string str = serialPort.ReadLine();
                            string[] splitChars = str.Split(new char[] { '%', ':', '#' }, StringSplitOptions.RemoveEmptyEntries);
                            if (splitChars[0] == "Speed")
                            {
                                speed = int.Parse(splitChars[1]);
                            }
                            else if (splitChars[0] == "Direction")
                            {
                                direction = int.Parse(splitChars[1]);
                            }
                            else if (splitChars[0] == "Height")
                            {
                                height = int.Parse(splitChars[1]);
                            }
                        }
                        catch (Exception ex)
                        {
                            print(ex);
                        }
                    }
                }
            });


        }
        catch (Exception ex)
        {
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }

            Debug.LogError(ex);
            return;
        }
    }
}