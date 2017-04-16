using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialLogger : MonoBehaviour
{
    private void Awake()
    {
        EventManager<PrintLogPacket>.Register(onRecievePrintPacket);
    }

    public void onRecievePrintPacket(PrintLogPacket packet)
    {
        Debug.Log(packet.message);
    }
}
