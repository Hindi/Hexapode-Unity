using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintLogPacket : PacketAbstract
{
    public string message;
}

public class PrintLogPacketHandler : IRecievedPacketHandler
{
    public void parsePacket(string message)
    {
        PrintLogPacket packet = new PrintLogPacket();
        packet.Code = SerialCode.PRINT_LOG;
        packet.message = message.Substring(2, message.Length - 2);
        EventManager<PrintLogPacket>.Trigger(packet);
    }
}