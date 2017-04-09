using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AX12Manager : MonoBehaviour
{
  [SerializeField]
  private List<AX12> ax12List;

    private void Awake()
    {
        EventManager<AX12GoToPacket>.Register(onRecieveGoToPacket);
    }

    public void onRecieveGoToPacket(AX12GoToPacket packet)
    {
        SetGoal(packet.Id - 1, packet.Angle);
    }

  private void SetGoal(int id, int goal)
  {
        if (id < ax12List.Count)
        {
            Debug.Log("AX12 " + id + " moved to " + goal);
            ax12List[id].SetGoal(goal);
        }
        else if (id == 0xFE)
            foreach (AX12 ax12 in ax12List)
                ax12.SetGoal(goal);
  }
}