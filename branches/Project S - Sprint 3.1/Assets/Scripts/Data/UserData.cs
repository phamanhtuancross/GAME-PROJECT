using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataKeys
{
    public const string HEART = "Heart";
    public const string HEART_TIME = "HeartTime";
    public const string GOLD = "Gold";
    public const string KEY = "Key";
    public const string NEXT_KEY_LEVEL = "NextKeyLevel";
    public const string MUSIC_VOLUME = "MusicVolume";
    public const string SFX_VOLUME = "SFXVolume";
    public const string BRUSH_ITEM = "BrushItem";
    public const string HAMMER_ITEM = "HammerItem";
    public const string CLOCK_ITEM = "ClockItem";
    public const string BRUSH_ITEM_TIME = "BrushItemTime";
    public const string HAMMER_ITEM_TIME = "HammerItemTime";
    public const string CLOCK_ITEM_TIME = "ClockItemTime";
    public const string DAILY_BONUS_WEEK = "BonusWeek";
    public const string DAILY_BONUS_DAY = "BonusDay";
    public const string DAILY_BONUS_CLAIMED_TIME = "BonusTime";
    public const string INF_HEART_TIME = "InfHeartTime";
    public const string INF_HEART_AMOUNT = "InfHeartAmount";
    public const string DATA_KEY_LEVEL_COMPLETE = "LevelComplete";
    public const string DATA_KEY_QUEST_COMPLETE = "QuestComplete";
}

public static class UserData
{
    #region GETTER
    static public int GetHeart()
    {
        if (!PlayerPrefs.HasKey(DataKeys.HEART))
            PlayerPrefs.SetInt(DataKeys.HEART, 5);
        int result = PlayerPrefs.GetInt(DataKeys.HEART);
        return result;
    }

    static public System.DateTime GetHeartTime()
    {
        string result = PlayerPrefs.GetString(DataKeys.HEART_TIME);
        if (result == "" || result == null)
        {
            result = System.DateTime.Now.ToString();
            PlayerPrefs.SetString(DataKeys.HEART_TIME, result);
        }
        return System.DateTime.Parse(result);
    }

    static public int GetGold()
    {
        int result = PlayerPrefs.GetInt(DataKeys.GOLD);
        return result;
    }

    static public int GetKey()
    {
        int result = PlayerPrefs.GetInt(DataKeys.KEY);
        return result;
    }

    static public int GetNextKeyLevel()
    {
        int result = PlayerPrefs.GetInt(DataKeys.NEXT_KEY_LEVEL);
        return result;
    }

    static public float GetMusicVolume()
    {
        float result = PlayerPrefs.GetFloat(DataKeys.MUSIC_VOLUME);
        return result;
    }

    static public float GetSFXVolume()
    {
        float result = PlayerPrefs.GetFloat(DataKeys.SFX_VOLUME);
        return result;
    }

    static public int GetItemCount(ItemsType type)
    {
        int result = 0;

        switch (type)
        {
            case ItemsType.NONE_ITEM:
                result = 0;
                break;
            case ItemsType.BRUSH_ITEM:
                result = PlayerPrefs.GetInt(DataKeys.BRUSH_ITEM);
                break;
            case ItemsType.HAMMER_ITEM:
                result = PlayerPrefs.GetInt(DataKeys.HAMMER_ITEM);
                break;
            case ItemsType.CLOCK_ITEM:
                result = PlayerPrefs.GetInt(DataKeys.CLOCK_ITEM);
                break;
            default:
                break;
        }

        return result;
    }

    static public System.DateTime GetItemTime(ItemsType type)
    {
        string result = "";

        switch (type)
        {
            case ItemsType.NONE_ITEM:
                result = System.DateTime.MaxValue.ToString();
                break;
            case ItemsType.BRUSH_ITEM:
                result = PlayerPrefs.GetString(DataKeys.BRUSH_ITEM_TIME);
                if (result == "" || result == null)
                {
                    result = System.DateTime.MaxValue.ToString();
                    PlayerPrefs.SetString(DataKeys.BRUSH_ITEM_TIME, result);
                }
                break;
            case ItemsType.HAMMER_ITEM:
                result = PlayerPrefs.GetString(DataKeys.HAMMER_ITEM_TIME);
                if (result == "" || result == null)
                {
                    result = System.DateTime.MaxValue.ToString();
                    PlayerPrefs.SetString(DataKeys.HAMMER_ITEM_TIME, result);
                }
                break;
            case ItemsType.CLOCK_ITEM:
                result = PlayerPrefs.GetString(DataKeys.CLOCK_ITEM_TIME);
                if (result == "" || result == null)
                {
                    result = System.DateTime.MaxValue.ToString();
                    PlayerPrefs.SetString(DataKeys.CLOCK_ITEM_TIME, result);
                }
                break;
            default:
                break;
        }

        return System.DateTime.Parse(result);
    }

    static public System.TimeSpan GetDailyBonusTimePassed()
    {
        System.DateTime timeLastClaim = System.DateTime.MinValue;

        if (PlayerPrefs.HasKey(DataKeys.DAILY_BONUS_CLAIMED_TIME))
        {
            timeLastClaim = System.DateTime.Parse(PlayerPrefs.GetString(DataKeys.DAILY_BONUS_CLAIMED_TIME));
        }
        else
        {
            PlayerPrefs.SetString(DataKeys.DAILY_BONUS_CLAIMED_TIME, timeLastClaim.ToString());
        }
        System.TimeSpan timePassed = System.DateTime.Now.Subtract(timeLastClaim);
        return timePassed;
    }

    static public int GetDailyBonusWeek()
    {
        int lastBonusWeek = PlayerPrefs.GetInt(DataKeys.DAILY_BONUS_WEEK);  //last claim
        System.TimeSpan timePassed = GetDailyBonusTimePassed();

        if (timePassed.TotalHours >= 48d)  //overdue
            return 1;
        if (timePassed.TotalHours >= 24d)   //new bonus
        {
            if (GetDailyBonusDay() == 1)
                return lastBonusWeek + 1;   //new week
            else
                return lastBonusWeek;
        }

        return lastBonusWeek;  //bonus claimed
    }

    static public int GetDailyBonusDay()
    {
        int lastBonusDay = PlayerPrefs.GetInt(DataKeys.DAILY_BONUS_DAY);  //last claim
        System.TimeSpan timePassed = GetDailyBonusTimePassed();

        if (timePassed.TotalHours >= 48d)  //overdue
            return 1;
        if (timePassed.TotalHours >= 24d)   //new bonus
        {
            if (lastBonusDay >= 7)  //new week
                return 1;
            else
                return lastBonusDay + 1;
        }

        return lastBonusDay;  //bonus claimed
    }

    public static System.TimeSpan GetInfHeartTimeLeft()
    {
        if(!PlayerPrefs.HasKey(DataKeys.INF_HEART_AMOUNT) || !PlayerPrefs.HasKey(DataKeys.INF_HEART_TIME))  //init data
        {
            PlayerPrefs.SetString(DataKeys.INF_HEART_AMOUNT, (new System.TimeSpan(0, 0, 0).ToString()));    //amount = 0
            PlayerPrefs.SetString(DataKeys.INF_HEART_TIME, System.DateTime.MinValue.ToString());    //timePass = max
        }

        System.TimeSpan currentAmount = System.TimeSpan.Parse(PlayerPrefs.GetString(DataKeys.INF_HEART_AMOUNT));
        System.DateTime timeLast = System.DateTime.Parse(PlayerPrefs.GetString(DataKeys.INF_HEART_TIME));
        System.TimeSpan timePassed = System.DateTime.Now.Subtract(timeLast);

        System.TimeSpan result = currentAmount - timePassed;
        return result;
    }

    public static int getLevelComplete()
    {
        int LevelComplete = PlayerPrefs.GetInt(DataKeys.DATA_KEY_LEVEL_COMPLETE, 0);
        Debug.Log(LevelComplete);
        return LevelComplete;
    }
    public static List<int> getCurrentQuest()
    {
        List<int> CurrentQuest = new List<int>();
        CurrentQuest = JsonUtility.FromJson<List<int>>(PlayerPrefs.GetString(DataKeys.DATA_KEY_QUEST_COMPLETE));
        if (CurrentQuest.Count == 0)
        {
            CurrentQuest = new List<int>();
            CurrentQuest.Add(1);
            updateCurrentQuest(CurrentQuest);
        }
        Debug.Log(CurrentQuest.Count);
        return CurrentQuest;
    }
    #endregion

    #region SETTER
    static public void AddHeart(bool resetTime = true)
    {
        int heart = Mathf.Clamp(GetHeart() + 1, 0, 5);
        PlayerPrefs.SetInt(DataKeys.HEART, heart);
        if (resetTime)
            PlayerPrefs.SetString(DataKeys.HEART_TIME, System.DateTime.Now.ToString());
    }

    static public void DecreaseHeart()
    {
        if (GetHeart() == 5)
        {
            int heart = Mathf.Clamp(GetHeart() - 1, 0, 5);
            PlayerPrefs.SetInt(DataKeys.HEART, heart);
            PlayerPrefs.SetString(DataKeys.HEART_TIME, System.DateTime.Now.ToString());
        }
        else
        {
            int heart = Mathf.Clamp(GetHeart() - 1, 0, 5);
            PlayerPrefs.SetInt(DataKeys.HEART, heart);
        }
    }

    static public void AddGold(int GoldCount)
    {
        PlayerPrefs.SetInt(DataKeys.GOLD, GetGold() + GoldCount);
    }

    static public void SubtractGold(int GoldCount)
    {
        PlayerPrefs.SetInt(DataKeys.GOLD, GetGold() - GoldCount);
    }

    static public void AddKey(int keyCount)
    {
        PlayerPrefs.SetInt(DataKeys.KEY, GetKey() + keyCount);
    }

    static public void SubtractKey(int keyCount)
    {
        PlayerPrefs.SetInt(DataKeys.KEY, GetKey() - keyCount);
    }

    static public void SetNextKeyLevel(int level)
    {
        PlayerPrefs.SetInt(DataKeys.NEXT_KEY_LEVEL, level);
    }

    static public void SetMusicVolume(float level)
    {
        PlayerPrefs.SetFloat(DataKeys.MUSIC_VOLUME, level);
    }

    static public void SetSFXVolume(float level)
    {
        PlayerPrefs.SetFloat(DataKeys.SFX_VOLUME, level);
    }

    static public void AddItem(ItemsType type, int count)
    {
        switch (type)
        {
            case ItemsType.NONE_ITEM:
                break;
            case ItemsType.BRUSH_ITEM:
                PlayerPrefs.SetInt(DataKeys.BRUSH_ITEM, GetItemCount(type) + count);
                PlayerPrefs.SetString(DataKeys.BRUSH_ITEM_TIME, System.DateTime.MaxValue.ToString());
                break;
            case ItemsType.HAMMER_ITEM:
                PlayerPrefs.SetInt(DataKeys.HAMMER_ITEM, GetItemCount(type) + count);
                PlayerPrefs.SetString(DataKeys.HAMMER_ITEM_TIME, System.DateTime.MaxValue.ToString());
                break;
            case ItemsType.CLOCK_ITEM:
                PlayerPrefs.SetInt(DataKeys.CLOCK_ITEM, GetItemCount(type) + count);
                PlayerPrefs.SetString(DataKeys.CLOCK_ITEM_TIME, System.DateTime.MaxValue.ToString());
                break;
            default:
                break;
        }
    }

    static public void SubtractItem(ItemsType type, int count)
    {
        switch (type)
        {
            case ItemsType.NONE_ITEM:
                break;
            case ItemsType.BRUSH_ITEM:
                PlayerPrefs.SetInt(DataKeys.BRUSH_ITEM, GetItemCount(type) - count);
                if (GetItemCount(type) <= 0)        //if item count equals 0, resetTime
                {
                    PlayerPrefs.SetString(DataKeys.BRUSH_ITEM_TIME, System.DateTime.Now.ToString());
                }
                break;
            case ItemsType.HAMMER_ITEM:
                PlayerPrefs.SetInt(DataKeys.HAMMER_ITEM, GetItemCount(type) - count);
                if (GetItemCount(type) <= 0)        //if item count equals 0, resetTime
                {
                    PlayerPrefs.SetString(DataKeys.HAMMER_ITEM_TIME, System.DateTime.Now.ToString());
                }
                break;
            case ItemsType.CLOCK_ITEM:
                PlayerPrefs.SetInt(DataKeys.CLOCK_ITEM, GetItemCount(type) - count);
                if (GetItemCount(type) <= 0)        //if item count equals 0, resetTime
                {
                    PlayerPrefs.SetString(DataKeys.CLOCK_ITEM_TIME, System.DateTime.Now.ToString());
                }
                break;
            default:
                break;
        }
    }

    public static void updateLevelComplete()
    {
        int lastLevelComplete = getLevelComplete();
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_LEVEL_COMPLETE, ++lastLevelComplete);
    }
    public static void updateCurrentQuest(List<int> quests)
    {
        PlayerPrefs.SetString(DataKeys.DATA_KEY_QUEST_COMPLETE, JsonUtility.ToJson(quests));
    }

    /// <summary>
    /// Add infinite heart in amount of time
    /// </summary>
    /// <param name="time">Amount of time</param>
    static public void AddInfHeart(System.TimeSpan time)
    {
        if(GetInfHeartTimeLeft() > System.TimeSpan.Zero)
        {
            System.TimeSpan currentAmount = System.TimeSpan.Parse(PlayerPrefs.GetString(DataKeys.INF_HEART_AMOUNT));
            PlayerPrefs.SetString(DataKeys.INF_HEART_AMOUNT, (currentAmount + time).ToString());
        }
        else
        {
            PlayerPrefs.SetString(DataKeys.INF_HEART_AMOUNT, time.ToString());
            PlayerPrefs.SetString(DataKeys.INF_HEART_TIME, System.DateTime.Now.ToString());
        }
    }

    static public void ClaimDailyBonus()
    {
        if (GetDailyBonusTimePassed().TotalHours < 24d) //bonus claimed
        {
            return;
        }

        PlayerPrefs.SetInt(DataKeys.DAILY_BONUS_WEEK, GetDailyBonusWeek());
        PlayerPrefs.SetInt(DataKeys.DAILY_BONUS_DAY, GetDailyBonusDay());
        PlayerPrefs.SetString(DataKeys.DAILY_BONUS_CLAIMED_TIME, System.DateTime.Now.ToString());
    }
    #endregion


}
