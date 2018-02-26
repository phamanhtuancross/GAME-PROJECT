using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Objective
{
    private int current;
    private int require;

    public Image Icon;
    public Text DetailText;
    public Image Tick;
    public GameObject gameObject;
    [HideInInspector]
    public bool IsSetup;
    [HideInInspector]
    public bool IsComplete;
    private bool isScoreLeveling;

    public bool IsScoreLeveling
    {
        get
        {
            return isScoreLeveling;
        }

        set
        {
            isScoreLeveling = value;
        }
    }

    public void Setup(Sprite _icon, int _require)
    {
        if (!IsSetup)
        {
            Icon.sprite = _icon;
            current = 0;
            require = _require;
            if (UIManager.Instance.isInfo)
                DetailText.text = require.ToString();
            else
            {
                if (!IsScoreLeveling)
                    DetailText.text = string.Format("{0}/{1}", current, require);
                else
                {
                    current = require;
                    DetailText.text = current.ToString();
                }
            }
            IsComplete = false;
            Tick.enabled = false;
            DetailText.enabled = true;
            IsSetup = true;

        }
    }

    public IEnumerator UpdateDetail(int complete, float TimeDelay = 0.01f)
    {
        UIManager.Instance.isCalculating = true;
        int add;
        if (complete >= 50)
            add = complete / 10;
        else
            add = 1;
        if (!IsScoreLeveling)
        {
            while (complete > 0 && current < require)
            {
                if (complete - add <= 0)
                    add = complete;
                complete -= add;
                current += add;
                if (current >= require)
                {
                    current = require;
                    //fix delete break
                }
                DetailText.text = string.Format("{0}/{1}", current, require);
                yield return new WaitForSeconds(TimeDelay);
            }

            if (current >= require && !IsComplete)
            {
                Tick.enabled = true;
                IsComplete = true;
                MissionManager.Instance.CheckLevelComplete();
            }
        } else
        {
            while (complete > 0 && current > 0)
            {
                if (complete - add <= 0)
                    add = complete;
                complete -= add;
                current -= add;
                if (current < 0)
                    current = 0;
                DetailText.text = current.ToString();
                yield return new WaitForSeconds(TimeDelay);
            }

            if (current <= 0 && !IsComplete)
            {
                DetailText.enabled = false;
                Tick.enabled = true;
                IsComplete = true;
                MissionManager.Instance.CheckLevelComplete();
            }
        }
        UIManager.Instance.isCalculating = false;
    }
}

public class UIManager : Singleton<UIManager>
{
    public GameSounds gameSounds;
    public UIDailyBonus DailyBonus;
    public bool isCalculating;
    private int step;
    private float timer;
    private int score;
    private int brushQuan;
    private int hammerQuan;
    private int clockQuan;
    private bool isDraggingBlock;
    private BlockShape blockSelected;
    private bool lockPlay;
    private uint exitCount;
    private Sprite QuestButtonImage;
    [HideInInspector]
    public bool isInfo;

    public int Step
    {
        get
        {
            return step;
        }
        set
        {
            step = value;
            if (MissionManager.Instance.IsStep)
            {
                StepText.text = "Step: " + step;
            }
        }
    }
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            int addScore = value - score;
            AddScoreText.text = addScore.ToString();

            if (addScore > 10)
            {
                score = value;
                StartCoroutine(UpdateScore(score - 10, score, ScoreText));
            }
            else
            {
                score = value;
                StartCoroutine(UpdateScore(score - addScore, score, ScoreText));
            }
        }
    }
    public int BrushQuan
    {
        set
        {
            brushQuan = value;
            BrushText.text = brushQuan.ToString();
        }
    }
    public int HammerQuan
    {
        set
        {
            hammerQuan = value;
            HammerText.text = hammerQuan.ToString();
        }
    }
    public int ClockQuan
    {
        set
        {
            clockQuan = value;
            ClockText.text = clockQuan.ToString();
        }
    }
    public float Timer
    {
        get
        {
            return timer;
        }

        set
        {
            timer = value;
            int Minite = (int)timer / 60;
            int Second = (int)timer - Minite * 60;
            StepText.text = string.Format("Time: {0}:{1}", Minite, Second);
        }
    }
    public int CoinReward
    {
        set
        {
            CoinRewardText.text = "x   " + value;
        }
    }
    public bool LockPlay
    {
        get
        {
            return lockPlay;
        }
    }

    [Header("Text")]
    public Text StepText;
    public Text ScoreText;
    public Text AddScoreText;
    public Text BrushText;
    public Text HammerText;
    public Text ClockText;
    public Text CoinRewardText;
    public Text KeyRewardText;
    public Text CurrentLevelText;
    public Text LevelTextIngame;

    [Header("Reference")]
    public GameObject LevelButtonPrefab;
    public GameObject PausePanel;
    public GameObject LevelFailPanel;
    public GameObject LevelInfoPanel;
    public GameObject QuestInfoPanel;
    public GameObject ShowLevelButton;
    public GameObject QuestButton;
    public Transform AddScorePos;
    public Image FaddingImage;
    public LevelCompletePanel LevelCompletePanel;
    public BuyLevelPanel BuyLevelPanel;
    public GameObject BuyLifePanel;

    [Header("UI Canvas")]
    public GameObject HomeUI;
    public GameObject GamePlayUI;

    [Header("GamePlay properties")]
    public GameObject CollectEffect;
    public BoardGameManager board;
    public BlockShape BlockShape_4x4_1;
    public BlockShape BlockShape_4x4_2;
    public BlockShape BlockShape_4x4_3;
    public List<Objective> MissionInfo = new List<Objective>();
    public List<Objective> MissionIngame = new List<Objective>();
    public LayerMask BlockLayer;

    // Use this for initialization
    void Start()
    {
        //multitouch not allowed
        Input.multiTouchEnabled = false;

        //show daily bonus
        if (UserData.GetDailyBonusTimePassed().TotalHours >= 24d)
            DailyBonus.gameObject.SetActive(true);
        else
            DailyBonus.gameObject.SetActive(false);

        QuestButtonImage = QuestButton.GetComponent<Image>().sprite;

        lockPlay = true;
        board.Create_Grid();
        board.Set_NullBlockOnBoard();
        Set_IndexForShapeHolder();
        board.ReCreate_BoardFromFileData();
        CurrentLevelText.text = (MissionManager.Instance.LevelComplete + 1).ToString();
        
    }


    public void Set_IndexForShapeHolder()
    {
        BlockShape_4x4_1.IndexShpaeHolder = ShapeHolderIndex.FIRST_INDEX;
        BlockShape_4x4_2.IndexShpaeHolder = ShapeHolderIndex.SECOND_INDEX;
        BlockShape_4x4_3.IndexShpaeHolder = ShapeHolderIndex.THREE_INDEX;
    }

    private void OnApplicationQuit()
    {
        bool IsGameOver = board.isGameOver;
        board.Save_BoardOnUserExitGame(isFailledMission: IsGameOver);
    }

    private void DisableExit()
    {
        exitCount = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitCount++;
            if (!IsInvoking("DisableExit"))
                Invoke("DisableExit", 0.3f);
        }

        if (exitCount == 2)
            Application.Quit();

        if (lockPlay)
            return;

        if (!isDraggingBlock)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hit;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.DrawRay(pos, Vector3.forward * 100, Color.red, 10f);
                hit = Physics2D.Raycast(pos, Vector3.forward, 100f, BlockLayer);
                if (hit.collider != null && hit.collider.tag == "Block")
                {
                    isDraggingBlock = true;
                    blockSelected = hit.collider.GetComponent<BlockShape>();
                    blockSelected.SelectBlock();
                }
            }
        }
        else
        {
            blockSelected.MoveBlock(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward * 10, Color.red, 10f);
            if (Input.GetMouseButtonUp(0))
            {
                blockSelected.PlaceBlock();
                isDraggingBlock = false;
                blockSelected = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (lockPlay)
            return;

        if (BlockShape_4x4_1.Block == BlockType.EMPTY &&
            BlockShape_4x4_2.Block == BlockType.EMPTY &&
            BlockShape_4x4_3.Block == BlockType.EMPTY)
            RandomHard();
    }

    [ContextMenu("RandomEasy")]
    public void RandomEasy()
    {
        List<BlockShape.ShapeDirectionPair> avoidBlocks = AvoidBlock.AvoidBlockEasy();
        List<BlockType> avoidBlockTypes = AvoidBlock.RandomBlockType();
        BlockShape_4x4_1.InitShape(avoidBlocks[0].shape, avoidBlocks[0].direction, avoidBlockTypes[0]);
        BlockShape_4x4_2.InitShape(avoidBlocks[1].shape, avoidBlocks[1].direction, avoidBlockTypes[1]);
        BlockShape_4x4_3.InitShape(avoidBlocks[2].shape, avoidBlocks[2].direction, avoidBlockTypes[2]);
    }
    [ContextMenu("RandomHard")]
    public void RandomHard()
    {
        List<BlockShape.ShapeDirectionPair> avoidBlocks = AvoidBlock.AvoidBlockHard(board.Grid);
        List<BlockType> avoidBlockTypes = AvoidBlock.RandomBlockType();
        BlockShape_4x4_1.InitShape(avoidBlocks[0].shape, avoidBlocks[0].direction, avoidBlockTypes[0]);
        BlockShape_4x4_2.InitShape(avoidBlocks[1].shape, avoidBlocks[1].direction, avoidBlockTypes[1]);
        BlockShape_4x4_3.InitShape(avoidBlocks[2].shape, avoidBlocks[2].direction, avoidBlockTypes[2]);
    }
    
    void LoadObjective(List<Objective> missionInfo)
    {
        MissionManager.Instance.SetCurrentMission(MissionManager.Instance.LevelComplete + 1);
        Mission mission = MissionManager.Instance.CurrentMission;


        for (int i = 0; i < missionInfo.Count; i++)
        {
            missionInfo[i].IsSetup = false;
        }

        List<Target> target = mission.Targets;

        for (int i = 0; i < target.Count; i++)
        {
            Sprite[] sprites = MissionManager.Instance.getTargetSprite(target[i].TargetType);
            Sprite sprite = null;

            switch (target[i].TargetType)
            {
                case TargetType.DestroyImpedient:
                    switch (target[i].BlockType)
                    {
                        case BlockType.BLOCK_BLUE:
                            sprite = sprites[0];
                            break;
                        case BlockType.BLOCK_GRASS:
                            sprite = sprites[1];
                            break;
                        default:
                            Debug.Log("Target + " + target[i].BlockType.ToString());
                            break;
                    }
                    break;
                case TargetType.EatBlockColor:
                    sprite = sprites[(int)target[i].BlockType - 2];
                    break;
                case TargetType.EatBlockLine:
                    sprite = sprites[(int)target[i].LineType];
                    break;
                case TargetType.PutBlockShape:
                    sprite = sprites[(int)target[i].ShapeType];
                    break;
                case TargetType.Scored:
                    sprite = sprites[0];
                    break;
                case TargetType.EatCombo:
                    sprite = sprites[target[i].ComboX - 2];
                    break;
                default:
                    Debug.Log("No Target");
                    break;
            }
            if (sprite != null)
            {
                if (target[i].TargetType == TargetType.Scored)
                    missionInfo[i].IsScoreLeveling = true;
                else
                    missionInfo[i].IsScoreLeveling = false;
                missionInfo[i].Setup(sprite, target[i].Amount);
            }
        }
        for (int i = 0; i < missionInfo.Count; i++)
        {
            missionInfo[i].gameObject.SetActive(true);
            if (!missionInfo[i].IsSetup)
            {
                missionInfo[i].gameObject.SetActive(false);
            }
        }
    }

    public void GameOver(bool isActive)
    {
        lockPlay = isActive;
        LevelFailPanel.SetActive(isActive);

        if (!isActive)
        {
            score = 0;
            ScoreText.text = "0";
        }
        else
        {
            int levelFailedCount = PlayerPrefs.GetInt(DataKeys.LEVEL_FAILED_COUNT, 0);
            PlayerPrefs.SetInt(DataKeys.LEVEL_FAILED_COUNT, levelFailedCount + 1);
            gameSounds.playFailedGameSFX();
            LevelFailPanel.GetComponent<Animator>().SetTrigger("Show");
            if(levelFailedCount >= (7 - (int)MissionManager.Instance.CurrentMission.difficult))
            {
                BuyLevelPanel.Show();
            }
        }
    }
    
    public void LevelComplete(bool isActive)
    {
        lockPlay = isActive;
        //LevelCompletePanel.SetActive(isActive);
        //LevelCompletePanel.gameObject.SetActive(isActive);
        UpdateButtonInfo();

        if (!isActive)
        {
            score = 0;
            ScoreText.text = "0";
            LevelCompletePanel.gameObject.SetActive(isActive);
        }
        else
        {
            gameSounds.playCompleteGameSFX();
            UserData.AddGold(MissionManager.Instance.getGold());
            if (UserData.GetInfHeartTimeLeft() < System.TimeSpan.Zero)
                UserData.AddHeart(false);
            UserData.AddKey(1);
            PlayerPrefs.SetInt(DataKeys.LEVEL_FAILED_COUNT, 0);

            StartCoroutine(LevelCompletePanel.EmitParticle());
        }
    }

    public void UpdateButtonInfo()
    {
        QuestButton.GetComponent<Image>().sprite = LevelCompletePanel.HomeIcon;
        CurrentLevelText.text = (MissionManager.Instance.LevelComplete + 1).ToString();
    }

    public void PlayButtonAnimation(string trigger)
    {
        ShowLevelButton.GetComponent<Animator>().SetTrigger(trigger);
        QuestButton.GetComponent<Animator>().SetTrigger(trigger);

        if (trigger == "In")
            CallDarkStateBackground(false);
    }

    public void HomeButton()
    {
        gameSounds.playHomeBGMusic();
        StartCoroutine(Fadding(HomeUI, false));
    }

    public void PauseButton(bool isActive)
    {
        if (isActive)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        lockPlay = isActive;
        PausePanel.SetActive(isActive);
    }

    private void ResetGameState()
    {
        board.Reset_Board();
        BlockShape_4x4_1.ResetShape();
        BlockShape_4x4_2.ResetShape();
        BlockShape_4x4_3.ResetShape();
        LoadObjective(MissionIngame);
    }

    public void RestartButton()
    {
        if(UserData.GetHeart() <= 0)
        {
            BuyLifePanel.SetActive(false);
            BuyLifePanel.SetActive(true);
        }
        else
        {
            UserData.DecreaseHeart();
            ResetGameState();
            GameOver(false);

            BlockShape_4x4_1.GetComponent<Collider2D>().enabled = true;
            BlockShape_4x4_2.GetComponent<Collider2D>().enabled = true;
            BlockShape_4x4_3.GetComponent<Collider2D>().enabled = true;
        }        
    }

    public IEnumerator UpdateScore(int oldScore, int newScore, Text scoreText)
    {
        while ((newScore + 1) != oldScore)
        {
            scoreText.text = "x\t" + oldScore;
            oldScore++;
            yield return new WaitForSeconds(.08f);
        }
    }

    public void ShowAddScoreText(List<GameObject> visibleBlock)
    {
        Animator anim = AddScoreText.GetComponent<Animator>();
        if (visibleBlock.Count == 1)
        {
            AddScorePos.position = visibleBlock[0].transform.position;
            anim.SetTrigger("Show");
            return;
        }

        Bounds bound = new Bounds(visibleBlock[0].transform.position, Vector3.zero);
        for (int i = 0; i < visibleBlock.Count; i++)
        {
            bound.Encapsulate(visibleBlock[i].transform.position);
        }

        AddScorePos.position = bound.center;
        anim.SetTrigger("Show");

        if(visibleBlock.Count >= 10)
        {
            Vector3 pos = new Vector3(AddScorePos.position.x, AddScorePos.position.y, -10);
            
            GameObject go = Instantiate(CollectEffect, pos, Quaternion.identity);
            Destroy(go, 3f);
        }
    }

    public void PlayButton()
    {
        isInfo = true;
        UpdateLevelInfo();
        if (!ShowLevelButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Out"))
            PlayButtonAnimation("Out");
        LevelInfoPanel.GetComponent<Animator>().SetTrigger("In");
        CallDarkStateBackground(true); // fix    
    }

    public void CallDarkStateBackground(bool isDark)
    {
        HomeUI.transform.Find("DarkState").gameObject.SetActive(isDark);
    }

    public void ShowQuestPanel()
    {
        PlayButtonAnimation("Out");

        if (!GamePlayUI.activeInHierarchy)
        {            
            QuestInfoPanel.GetComponent<Animator>().SetTrigger("In");
        }
        else
        {
            gameSounds.playHomeBGMusic();
            StartCoroutine(Fadding(HomeUI, false));
        }
    }

    public void StartLevelButton()
    {
        gameSounds.playPlayBGMusic();
        isInfo = false;
        SetUpGamePlay();
        if (!ShowLevelButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Out"))
            CallDarkStateBackground(false); // fix        
        StartCoroutine(Fadding(GamePlayUI, true));
        if (UserData.GetInfHeartTimeLeft() < System.TimeSpan.Zero)
            UserData.DecreaseHeart();

        BlockShape_4x4_1.GetComponent<Collider2D>().enabled = true;
        BlockShape_4x4_2.GetComponent<Collider2D>().enabled = true;
        BlockShape_4x4_3.GetComponent<Collider2D>().enabled = true;
    }

    public void UpdateLevelInfo()
    {
        LevelInfoPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "Level " + (MissionManager.Instance.LevelComplete + 1);
        //UpdateItemCount(); //fix
        LoadObjective(MissionInfo);
        CoinReward = MissionManager.Instance.getGold();
    }
    
    private void ShowUI(GameObject activeUI)
    {
        HomeUI.SetActive(false);
        GamePlayUI.SetActive(false);

        activeUI.SetActive(true);
    }

    private void SetUpGamePlay()
    {
        PausePanel.SetActive(false);
        LevelFailPanel.SetActive(false);
        LevelCompletePanel.gameObject.SetActive(false);
    }

    private IEnumerator Fadding(GameObject go, bool canPlay)
    {
        FaddingImage.enabled = true;
        while (FaddingImage.color.a < 0.9f)
        {
            FaddingImage.color = Color.Lerp(FaddingImage.color, Color.black, .1f);
            yield return new WaitForSeconds(.01f);
        }
        FaddingImage.color = Color.black;

        yield return new WaitForSeconds(.5f);
        ShowUI(go);
        ResetGameState();

        if (canPlay)
            LoadObjective(MissionIngame);
        else
            QuestButton.GetComponent<Image>().sprite = QuestButtonImage;

        while (FaddingImage.color.a > 0.1f)
        {
            FaddingImage.color = Color.Lerp(FaddingImage.color, Color.clear, .1f);
            yield return new WaitForSeconds(.01f);
        }
        FaddingImage.color = Color.clear;
        FaddingImage.enabled = false;
        lockPlay = !canPlay;

        if (HomeUI.activeInHierarchy)
            PlayButtonAnimation("In");
    }
}
