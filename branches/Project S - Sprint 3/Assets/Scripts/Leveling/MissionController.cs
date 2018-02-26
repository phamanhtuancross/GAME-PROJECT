using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : Singleton<MissionController>
{

    public List<Mission> lMission = new List<Mission>();
    private Dictionary<int, Mission> lMissionLevel = null;

    public Dictionary<int, Mission> LMissionLevel
    {
        get
        {
            if (lMissionLevel != null)
                return lMissionLevel;
            else
            {
                lMissionLevel = initListMissionData();
                return lMissionLevel;
            }
        }
    }

    // init List MissionData
    private Dictionary<int, Mission> initListMissionData()
    {
        Dictionary<int, Mission> result = new Dictionary<int, Mission>();
       
        for (int i = 0; i < lMission.Count; i++)
        {
            if (lMission[i] != null)
                result.Add(result.Count + 1, lMission[i]);
        }
        return result;
    }
}
