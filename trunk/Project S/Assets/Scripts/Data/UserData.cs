using UnityEngine;
using System.Collections;

public static class DataKeys
{
    public const string DATA_KEY_HEART = "DataKeyHeart";
    public const string DATA_KEY_HEART_TIME = "DataKeyHeartTime";
    public const string DATA_KEY_GOLD = "DataKeyGold";
    public const string DATA_KEY_KEY = "DataKeyKey";
    public const string DATA_KEY_NEXT_KEY_LEVEL = "DataKeyNextKeyLevel";
    public const string DATA_KEY_MUSIC_VOLUME = "DataKeyMusicVolume";
    public const string DATA_KEY_SFX_VOLUME = "DataKeySFXVolume";
}

public static class UserData
{
    static public int GetHeart()
    {
        int result = PlayerPrefs.GetInt(DataKeys.DATA_KEY_HEART);
        return result;
    }

    static public System.DateTime GetHeartTime()
    {
        string result = PlayerPrefs.GetString(DataKeys.DATA_KEY_HEART_TIME);
        if (result == "" || result == null)
        {
            result = System.DateTime.Now.ToString();
            PlayerPrefs.SetString(DataKeys.DATA_KEY_HEART_TIME, result);
        }
        return System.DateTime.Parse(result);
    }

    static public int GetGold()
    {
        int result = PlayerPrefs.GetInt(DataKeys.DATA_KEY_GOLD);
        return result;
    }

    static public int GetKey()
    {
        int result = PlayerPrefs.GetInt(DataKeys.DATA_KEY_KEY);
        return result;
    }

    static public int GetNextKeyLevel()
    {
        int result = PlayerPrefs.GetInt(DataKeys.DATA_KEY_NEXT_KEY_LEVEL);
        return result;
    }

    static public void AddHeart()
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_HEART, GetHeart() + 1);
        PlayerPrefs.SetString(DataKeys.DATA_KEY_HEART_TIME, System.DateTime.Now.ToString());
    }

    static public void DecreaseHeart()
    {
        if(GetHeart() == 5)
        {
            PlayerPrefs.SetInt(DataKeys.DATA_KEY_HEART, GetHeart() - 1);
            PlayerPrefs.SetString(DataKeys.DATA_KEY_HEART_TIME, System.DateTime.Now.ToString());
        }
        else
        {
            PlayerPrefs.SetInt(DataKeys.DATA_KEY_HEART, GetHeart() - 1);
        }
    }

    static public void AddGold(int GoldCount)
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_GOLD, GetGold() + GoldCount);
    }

    static public void SubtractGold(int GoldCount)
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_GOLD, GetGold() - GoldCount);
    }

    static public void AddKey(int keyCount)
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_KEY, GetKey() + keyCount);
    }

    static public void SubtractKey(int keyCount)
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_KEY, GetKey() - keyCount);
    }

    static public void SetNextKeyLevel(int level)
    {
        PlayerPrefs.SetInt(DataKeys.DATA_KEY_NEXT_KEY_LEVEL, level);
    }

    static public float GetMusicVolume()
    {
        float result = PlayerPrefs.GetFloat(DataKeys.DATA_KEY_MUSIC_VOLUME);
        return result;
    }

    static public float GetSFXVolume()
    {
        float result = PlayerPrefs.GetFloat(DataKeys.DATA_KEY_SFX_VOLUME);
        return result;
    }

    static public void SetMusicVolume(float level)
    {
        PlayerPrefs.SetFloat(DataKeys.DATA_KEY_MUSIC_VOLUME, level);
    }

    static public void SetSFXVolume(float level)
    {
        PlayerPrefs.SetFloat(DataKeys.DATA_KEY_SFX_VOLUME, level);
    }
}
