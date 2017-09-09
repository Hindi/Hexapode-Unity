using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class Serial : MonoBehaviour
{
  [SerializeField]
  private Dropdown serialPicker;
  [SerializeField]
  private int baudRate = 1000000;

  PacketsHandler packetHandler;

  SerialPort serialPort;
  List<string> portsNames;
  private string currentPortSelected;
  Coroutine coroutine;

    private void Awake()
    {
        packetHandler = new PacketsHandler();
        portsNames = new List<string>();
    }

    // Use this for initialization
    void Start()
  {
    string[] ports = SerialPort.GetPortNames();
    portsNames.Clear();
    foreach (string s in ports)
    {
      portsNames.Add(s);
    }
    serialPicker.AddOptions(portsNames);
    serialPicker.onValueChanged.AddListener(OnClickSerialName);
  }

  public void OnClickSerialName(int value)
  {
    if (serialPicker.value == 0)
      return;

    currentPortSelected = portsNames[value];
    if (serialPort != null)
    {
      serialPort.Close();
    }
    serialPort = new SerialPort(currentPortSelected, baudRate, Parity.None, 8, StopBits.One);
    serialPort.ReadTimeout = 50; // in ms
    serialPort.NewLine = "\r"; //La stm32 attend un '\r' en fin de ligne, et non un "\r\n"
    try
    {
      serialPort.Open();
      if (coroutine != null)
        StopCoroutine(coroutine);

      coroutine = StartCoroutine(listenPort());
    }
    catch (Exception e)
    {
      Debug.Log(e);
    }

  }

  private IEnumerator listenPort()
  {
    string[] received = new string[0];
    while (true)
    {
      try
      {
        //serialPort.WriteLine("test");
        string read = serialPort.ReadLine();
        packetHandler.processPacket(read);
      }
      catch (Exception e)
      {
        if(!e.GetType().IsAssignableFrom(typeof(System.TimeoutException)))
        {
            Debug.Log(e);
        }
      }

      yield return new WaitForSeconds(0.01f);
    }
  }

  void printf(string s)
  {
    serialPort.WriteLine(s);
  }

  void OnApplicationQuit()
  {
    if (serialPort != null)
    {
      serialPort.Close();
    }
  }
}