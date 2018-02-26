using UnityEngine;
//using UnityEditor;

[System.Serializable]
public struct PipeLog
{
    public float PosY;
    public int Frame;
}

[System.Serializable]
public struct ItemLog
{
    public float PosY;
    public int ItemType;
    public int Frame;
}

[System.Serializable]
public struct TokenLog
{
    public float PosY;
    public int Count;
    public int Frame;
}

[System.Serializable]
public class RecordData
{
    public int Score;
    public long RecordTime; //use datetime.ToFileTimeUtc() to convert datetime to long, reverse long to date time using DateTime.FromFileTimeUtc(RecordTime.value)
    public int[] BirdFlyLogs;
    public PipeLog[] PipeSpawnLogs;
    public ItemLog[] ItemLogs;
    public TokenLog[] TokenLogs;
}

[System.Serializable]
public class AllRecordData
{
    public RecordData[] Records;
}