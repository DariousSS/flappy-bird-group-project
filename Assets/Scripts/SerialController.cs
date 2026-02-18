using UnityEngine;
using System.IO.Ports;
using System;

public class SerialController : MonoBehaviour
{
    SerialPort serialPort;

    public static int soundLevel;

    [SerializeField] string portName = "/dev/cu.usbmodem48CA433FDF242";
    [SerializeField] int baudRate = 57600;

    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("Available Ports: " + string.Join(", ", ports));

        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 100;
        serialPort.NewLine = "\n";

        try
        {
            serialPort.Open();
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            Debug.Log("Connected: " + portName);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to open port: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort == null || !serialPort.IsOpen) return;

        try
        {
            string data = serialPort.ReadLine();
            data = data.Trim();

            if (data.Length == 0) return;

            string[] values = data.Split(',');

            if (values.Length >= 4)
            {
                int value;
                if (int.TryParse(values[3], out value))
                {
                    soundLevel = value;
                    Debug.Log("FreqBand: " + soundLevel);
                }
            }
        }
        catch (TimeoutException)
        {
        }
        catch (Exception e)
        {
            Debug.Log("Read Error: " + e.Message);
        }
    }



    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
