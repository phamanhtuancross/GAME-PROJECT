using UnityEngine;
//using UnityEditor;

public class RecordPlayer : Singleton<RecordPlayer>
{
    private RecordData _record;
    private bool _isPlaying;
    private int FrameFixedUpdate;    //current frame FixedUpdate Number

    //indexing
    private int BirdLogIndex;   //index of next Bird Fly frame
    private int PipeLogIndex;   //index of next Pipe Spawn frame
    private int ItemLogIndex;   //index of next Item Spawn frame
    private int TokenLogIndex;  //index of next Token Spawn frame

    public bool IsPlaying
    {
        get
        {
            return _isPlaying;
        }

        set
        {
        }
    }

    private void Awake()
    {
        _isPlaying = false;
        FrameFixedUpdate = 0;
    }

    public void StartPlayingRecord(RecordData data)
    {
        _isPlaying = true;
        _record = data;
        FrameFixedUpdate = 0;
        BirdLogIndex = 0;
        PipeLogIndex = 0;
        ItemLogIndex = 0;
        TokenLogIndex = 0;

        //set Bird
        BirdController.Instance.IsAlive = true;
        BirdController.Instance.IsPlayingRecord = true;

        //set spawner pipe
        SpawnerPipe.Instance.IsPlayingRecord = true;
        SpawnerPipe.Instance.IsPlaying = true;

    }

    public void StopPlayingRecord()
    {
        _isPlaying = false;
        SpawnerPipe.Instance.IsPlayingRecord = false;
        FrameFixedUpdate = 0;
    }

    private void FixedUpdate()
    {
        if (!_isPlaying)
            return;

        ///NOTE: BirdController FixedUpdate being called after SpawnerPipe FixedUpdate, so we need to check BirdFlyLogs after SpawnerLogs

        if (PipeLogIndex < _record.PipeSpawnLogs.Length)
        {
            if (FrameFixedUpdate == _record.PipeSpawnLogs[PipeLogIndex].Frame)
            {
                //Pipe spawn at this frame
                SpawnerPipe.Instance.SpawnPipe(_record.PipeSpawnLogs[PipeLogIndex].PosY);
                PipeLogIndex++;
            }
        }

        if (ItemLogIndex < _record.ItemLogs.Length)
        {
            if (FrameFixedUpdate == _record.ItemLogs[ItemLogIndex].Frame)
            {
                //Item spawn at this frame
                SpawnerPipe.Instance.SpawnItem(_record.ItemLogs[ItemLogIndex].PosY, _record.ItemLogs[ItemLogIndex].ItemType);
                ItemLogIndex++;
            }
        }

        if (TokenLogIndex < _record.TokenLogs.Length)
        {
            if (FrameFixedUpdate == _record.TokenLogs[TokenLogIndex].Frame)
            {
                //Item spawn at this frame
                SpawnerPipe.Instance.SpawnToken(_record.TokenLogs[TokenLogIndex].PosY, _record.TokenLogs[TokenLogIndex].Count);
                TokenLogIndex++;
            }
        }

        if (BirdLogIndex < _record.BirdFlyLogs.Length)
        {
            if (FrameFixedUpdate == _record.BirdFlyLogs[BirdLogIndex])
            {
                //Bird flap at this frame
                BirdController.Instance.FlapUp();
                BirdLogIndex++;
            }
        }

        FrameFixedUpdate++;
    }
}