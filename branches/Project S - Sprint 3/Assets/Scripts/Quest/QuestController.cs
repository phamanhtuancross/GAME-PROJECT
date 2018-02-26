using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : Singleton<QuestController> {
    [SerializeField]
    private List<Quest> lQuestInput = new List<Quest>();
    private Dictionary<int, Quest> lQuest = null;

    public Dictionary<int, Quest> LQuest
    {
        get
        {
            if(lQuest == null)
            {
                InitQuestDictionary();
            }
            return lQuest;
        }
    }

    private void InitQuestDictionary()
    {
        lQuest = new Dictionary<int, Quest>();
        for(int i = 0; i < lQuestInput.Count; i++)
        {
            lQuest.Add(lQuestInput[i].STT, lQuestInput[i]);  
        }
    }
}
