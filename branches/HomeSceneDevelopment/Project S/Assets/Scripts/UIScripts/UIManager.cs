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

    public void Setup(Sprite _icon, int _require)
    {
        if (!IsSetup)
        {
            Icon.sprite = _icon;
            current = _require;
            require = _require;
            DetailText.text = current.ToString();
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
        if (complete >= 20) 
            add = 20;
        else
            add = 1;
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
        UIManager.Instance.isCalculating = false;
    }
}

public class UIManager : Singleton<UIManager> {
    public SFXSounds sfxSounds;
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

            if(addScore > 10)
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

    [Header("Text")]
    public Text StepText;
    public Text ScoreText;
    public Text AddScoreText;
    public Text BrushText;
    public Text HammerText;
    public Text ClockText;
    public Text CoinRewardText;
    public Text KeyRewardText;
    public Transform AddScorePos;

    [Header("Panel")]
    public GameObject LevelButtonPrefab;
    public GameObject PausePanel;
    public GameObject LevelFailPanel;
    public GameObject LevelCompletePanel;
    public LevelCompletePanel LevelCompleteScript;
    public GameObject LevelInfoPanel;
    public GameObject SelectLevelPanel;
    public Image FaddingImage;

    [Header("UI Canvas")]
    public GameObject HomeUI;
    public GameObject GamePlayUI;

    [Header("GamePlay properties")]
    public BoardGameManager board;
    public BlockShape BlockShape_4x4_1;
    public BlockShape BlockShape_4x4_2;
    public BlockShape BlockShape_4x4_3;
    public List<Objective> MissionInfo = new List<Objective>();
    public List<Objective> MissionIngame = new List<Objective>();
    public LayerMask BlockLayer;

	// Use this for initialization
	void Start ()
    {
        lockPlay = true;
        board.Create_Grid();
        LoadLevel();
    }

    private void LoadLevel()
    {       
        for (int i = 0; i < MissionController.Instance.LMissionLevel.Count; i++)
        {
            GameObject button = Instantiate(LevelButtonPrefab, SelectLevelPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0)); ///child of content scrollview
            int temp = i + 1;

            if (temp <= MissionManager.Instance.LevelComplete)
            {
                button.GetComponent<LevelData>().Complete(temp);
            }
            else
            { 
                if(temp == MissionManager.Instance.LevelComplete + 1)
                    button.GetComponent<LevelData>().Unlock(temp);
                else
                    button.GetComponent<LevelData>().Lock();
            }
            button.GetComponent<Button>().onClick.AddListener(() => { ShowLevelInfoPanel(true, temp); });
            button.GetComponent<Button>().onClick.AddListener(() => { sfxSounds.playButtonClickSFX(); });
        }
    }

    // Update is called once per frame
    void Update () {
        if (lockPlay)
            return;

		if(!isDraggingBlock)
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
        MissionManager.Instance.SetCurrentMission(MissionManager.Instance.CurrentLevel);
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
			sfxSounds.playFailedGameSFX();
            LevelFailPanel.GetComponent<Animator>().SetTrigger("Show");
        }    }
    
    public void LevelComplete(bool isActive)
    {
        lockPlay = isActive;
        //LevelCompletePanel.SetActive(isActive);
        LevelCompleteScript.gameObject.SetActive(isActive);

        if (!isActive)
        {
            score = 0;
            ScoreText.text = "0";
        }
        else
        {
            LevelCompleteScript.GetComponent<Animator>().SetTrigger("Show");
            sfxSounds.playCompleteGameSFX();
            UserData.AddGold(MissionManager.Instance.getGold());
            UserData.AddHeart();
            UserData.AddKey(1);
            //if ((MissionManager.Instance.CurrentLevel + 1) % 10 == 0 && MissionManager.Instance.CurrentLevel >= UserData.GetNextKeyLevel())
            //{
            //    UserData.AddKey(1);
            //    UserData.SetNextKeyLevel(MissionManager.Instance.CurrentLevel + 10);
            //}
        }
    }

    public void HomeButton()
    {
        LevelComplete(false);
        UpdateLevel();
        StartCoroutine(Fadding(HomeUI, false));
    }

    private void UpdateLevel()
    {
        GameObject LevelContainer = SelectLevelPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; ///child of content scrollview
        for (int i = 0; i < LevelContainer.transform.childCount; i++)
        {
            GameObject button = LevelContainer.transform.GetChild(i).gameObject;

            int temp = i + 1;

            if (temp <= MissionManager.Instance.LevelComplete)
            {
                button.GetComponent<LevelData>().Complete(temp);
            }
            else
            {
                if (temp == MissionManager.Instance.LevelComplete + 1)
                    button.GetComponent<LevelData>().Unlock(temp);
                else
                    button.GetComponent<LevelData>().Lock();
            }
            button.GetComponent<Button>().onClick.AddListener(() => { ShowLevelInfoPanel(true, temp); });
        }
        
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
        ResetGameState();
        GameOver(false);
    }

    public IEnumerator UpdateScore(int oldScore, int newScore, Text scoreText)
    {
        while((newScore + 1) != oldScore)
        {
            scoreText.text = oldScore.ToString();
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
    }

    public void PlayButton()
    {  
        ShowLevelInfoPanel(false, 0);
    }

    public void StartLevelButton()
    {
        ShowLevelInfoPanel(false, 0);
        StartCoroutine(Fadding(GamePlayUI, true));
        UserData.DecreaseHeart();
    }

    public void ShowLevelInfoPanel(bool isActive, int level)
    {
        if (level > MissionManager.Instance.LevelComplete+1)
            return;

        if (level != 0)
        {
            LevelInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Level " + level;
            MissionManager.Instance.CurrentLevel = level;
            LoadObjective(MissionInfo);
            CoinReward = MissionManager.Instance.getGold();
        }

        if (isActive)
            GetComponent<Animator>().SetTrigger("Show");
        else
        {
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LevelInfoPopout"))
                GetComponent<Animator>().SetTrigger("Hide");
        }
        //LevelInfoPanel.SetActive(isActive);
    }

    private void ShowUI(GameObject activeUI)
    {
        HomeUI.SetActive(false);
        GamePlayUI.SetActive(false);

        activeUI.SetActive(true);
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

        ShowUI(go);

        ResetGameState();
        LoadObjective(MissionIngame);

        while (FaddingImage.color.a > 0.1f)
        {
            FaddingImage.color = Color.Lerp(FaddingImage.color, Color.clear, .1f);
            yield return new WaitForSeconds(.01f);
        }
        FaddingImage.color = Color.clear;
        FaddingImage.enabled = false;
        lockPlay = !canPlay;

    }
}
