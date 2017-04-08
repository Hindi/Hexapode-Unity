using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecievedPacketHandler
{
    void parsePacket(string message);
}
