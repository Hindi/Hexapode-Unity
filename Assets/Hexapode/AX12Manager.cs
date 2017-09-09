using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LegListWrapper
{
  public List<AX12> ax12List;
}

public class AX12Manager : MonoBehaviour
{
    [SerializeField]
    private List<LegListWrapper> legList;

    private void Awake()
    {
        EventManager<AX12GoToPacket>.Register(onRecieveGoToPacket);
    }

    public void onRecieveGoToPacket(AX12GoToPacket packet)
    {
        SetGoal(packet.Id, packet.Angle);
    }

    private void SetGoal(int id, int goal)
    {
        int legId = id / 10;
        int ax12Id = id % 10 - 1;
        if (legId < legList.Count && ax12Id < legList[legId].ax12List.Count)
        {
            Debug.Log("AX12 " + id + " moved to " + goal);
            legList[legId].ax12List[ax12Id].SetGoal(goal);
        }

        else if (id == 0xFE)
            foreach (LegListWrapper leg in legList)
                foreach (AX12 ax12 in leg.ax12List)
                    ax12.SetGoal(goal);
    }
}