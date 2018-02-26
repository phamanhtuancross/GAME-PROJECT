using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recorder : Singleton<Recorder>
{
    private List<int> BirdFlyLogs;
    private List<PipeLog> PipeSpawnLogs;
    private List<ItemLog> ItemLogs;
    private List<TokenLog> TokenLogs;

    private int FrameFixedUpdate;
    private bool isRecording;

    private void Awake()
    {
        BirdFlyLogs = new List<int>();
        PipeSpawnLogs = new List<PipeLog>();
        ItemLogs = new List<ItemLog>();
        TokenLogs = new List<TokenLog>();
        FrameFixedUpdate = 0;
        isRecording = false;
        GameData.Instance.LoadRecords();
    }

    private void FixedUpdate()
    {
        //increase FrameFixedUpdate each frame
        if (!isRecording)
            return;
        FrameFixedUpdate++;
    }

    /// <summary>
    /// Call this func only when Bird begin fly up
    /// </summary>
    public void AddBirdFlyLog()
    {
        if (!isRecording)
            return;
        BirdFlyLogs.Add(FrameFixedUpdate);
    }

    /// <summary>
    /// Call this func only when Spawn new Pipe
    /// </summary>
    /// <param Position on Y Axes of PipeHolder="PositionY"></param>
    public void AddPipeSpawnLog(float PositionY)
    {
        if (!isRecording)
            return;
        PipeLog newLog = new PipeLog();
        newLog.PosY = PositionY;
        newLog.Frame = FrameFixedUpdate;
        PipeSpawnLogs.Add(newLog);
    }

    /// <summary>
    /// Call this func only when Spawn new Item
    /// </summary>
    /// <param name="PosY"></param>
    /// <param name="tag"></param>
    public void AddItemLog(float PosY, int ItemType)
    {
        if (!isRecording)
            return;
        ItemLog newLog = new ItemLog();
        newLog.PosY = PosY;
        newLog.ItemType = ItemType;
        newLog.Frame = FrameFixedUpdate;
        ItemLogs.Add(newLog);
    }

    /// <summary>
    /// Call this func only when spawn a group of tokens
    /// </summary>
    /// <param name="PosY"></param>
    /// <param name="count"></param>
    public void AddTokenLog(float PosY, int count)
    {
        if (!isRecording)
            return;
        TokenLog newLog = new TokenLog();
        newLog.PosY = PosY;
        newLog.Count = count;
        newLog.Frame = FrameFixedUpdate;
        TokenLogs.Add(newLog);
    }

    /// <summary>
    /// Call this func when player touch start record button
    /// </summary>
    public void StartRecord()
    {
        if (isRecording)
            return;
        FrameFixedUpdate = 0;
        BirdFlyLogs = new List<int>();
        PipeSpawnLogs = new List<PipeLog>();
        ItemLogs = new List<ItemLog>();
        TokenLogs = new List<TokenLog>();
        isRecording = true;

        //Set mode for Bird and SpawnerPipe
        BirdController.Instance.RecordMode = true;
        SpawnerPipe.Instance.RecordMode = true;
    }

    /// <summary>
    /// Stop record and save record
    /// </summary>
    public void StopRecord()
    {
        if (!isRecording)
            return;
       
        FrameFixedUpdate = 0;
        isRecording = false;

        //Set mode for Bird and SpawnerPipe
        BirdController.Instance.RecordMode = false;
        SpawnerPipe.Instance.RecordMode = false;

        if (!RecordPlayer.Instance.IsPlaying)
            SaveRecord();
    }

    /// <summary>
    /// Save Record After Stopped Record (Optional)
    /// </summary>
    public void SaveRecord()
    {
        if (isRecording)
            return;
        RecordData newRecord = new RecordData();
        newRecord.Score = UIManager.Instance.GetScore();
        newRecord.RecordTime = System.DateTime.Now.ToFileTimeUtc();
        newRecord.BirdFlyLogs = BirdFlyLogs.ToArray();
        newRecord.PipeSpawnLogs = PipeSpawnLogs.ToArray();
        newRecord.ItemLogs = ItemLogs.ToArray();
        newRecord.TokenLogs = TokenLogs.ToArray();

        GameData.Instance.AddRecord(newRecord);
    }
}
