using UnityEngine;
using System.Collections;

public enum BonusType
{
    LIFE,
    GOLD,
    KEY,
    ITEM_BRUSH,
    ITEM_HAMMER,
    ITEM_CLOCK
}

[System.Serializable]
public class DailyBonusData
{
    public int dayNumber;
    public int count;
    public string title;
    public BonusType type;
    public Sprite bonusSprite;
}

[CreateAssetMenu(fileName = "New Weekly Bonus Data", menuName = "Weekly Bonus")]
//[System.Serializable]
public class WeeklyBonusData : ScriptableObject
{
    public int weekNumber;
    public DailyBonusData[] weekBonus;
}
