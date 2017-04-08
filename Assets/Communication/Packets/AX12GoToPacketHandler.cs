using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AX12GoToPacket : PacketAbstract
{
    public int Angle;
    public int Id;
}

public class AX12GoToPacketHandler : IRecievedPacketHandler
{
    public void parsePacket(string message)
    {
        AX12GoToPacket packet = new AX12GoToPacket();
        packet.Code = SerialCode.AX12_GOTO;
        packet.Id = Convert.ToInt32(message.Substring(2, 2), 16);
        packet.Angle = Convert.ToInt32(message.Substring(4, 2), 16);
        EventManager<AX12GoToPacket>.Trigger(packet);
    }
}
