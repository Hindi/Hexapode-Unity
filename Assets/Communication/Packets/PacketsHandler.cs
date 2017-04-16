using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SerialCode
{
    PRINT_LOG = 0x00,
    AX12_GOTO = 0x01

}

public class PacketsHandler
{
    private Dictionary<SerialCode, IRecievedPacketHandler> packetTemplates;

    public PacketsHandler()
    {
        packetTemplates = new Dictionary<SerialCode, IRecievedPacketHandler>();
        packetTemplates.Add(SerialCode.AX12_GOTO, new AX12GoToPacketHandler());
        packetTemplates.Add(SerialCode.PRINT_LOG, new PrintLogPacketHandler());
    }

    public void processPacket(string packet)
    {
        int code = Convert.ToInt32(packet.Substring(0, 2), 16);
        packetTemplates[(SerialCode)code].parsePacket(packet);
    }
}
