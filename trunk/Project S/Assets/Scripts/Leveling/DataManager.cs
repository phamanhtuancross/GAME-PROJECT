using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public static int getLevelComplete()
    {
        int LevelComplete = PlayerPrefs.GetInt("LevelComplete", 0);
        return LevelComplete;
    }

    public static void updateLevelComplete()
    {
        int lastLevelComplete = getLevelComplete();
        PlayerPrefs.SetInt("LevelComplete", ++lastLevelComplete);
    }
}
