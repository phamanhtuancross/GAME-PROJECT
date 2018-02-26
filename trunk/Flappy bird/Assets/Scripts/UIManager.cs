using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject Main, Ingame, Result, RecordList, LogoutPanel;
    public Text scoreIngame, scoreResult, highscore, coin;
    public SpawnerPipe spawnPipe;
    public GameObject btnSaveRecord;
    public Button btnFBLogin, btnRecordList, btnShop, btnStart;
    public ContentClipList contentList;
    public float scorecoin;

    [HideInInspector] public Rigidbody2D birdRB;
    [HideInInspector] public BirdController bird;
    
    private void Awake()
    {
        birdRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        bird = birdRB.GetComponent<BirdController>();
    }

    // Use this for initialization
    void Start () {
        birdRB.constraints = RigidbodyConstraints2D.FreezePositionY;
        RestartButton();
    }
	
	// Update is called once per frame
	void Update () {
        if (!Ingame.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else
            {
                return;
            }
        }
        
        if(BirdController.Instance.IsPlayingRecord && Input.GetKeyDown(KeyCode.Escape))
        {
            bird.IsAlive = false;
            spawnPipe.IsPlaying = false;

            RecordPlayer.Instance.StopPlayingRecord();
            ParallaxBackground.Instance.setRunningState(false);

            birdRB.velocity = Vector2.zero;
            RestartButton();
            bird.OnButtonRestartClick();
        }

        if (Input.GetMouseButtonDown(0) && Ingame.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            //Start record
            Recorder.Instance.StartRecord();

            ShowInstruction(false);
            birdRB.constraints = RigidbodyConstraints2D.None;
            bird.enabled = true;
            spawnPipe.IsPlaying = true;
        }

        if (!bird.IsAlive)
        {
            spawnPipe.IsPlaying = false;
            //Time.timeScale = 0;
            birdRB.velocity = Vector2.zero;
            ShowResult();
        }

    }

    public void StartButton()
    {
        Main.SetActive(false);
        Ingame.SetActive(true);
        Result.SetActive(false);
        BirdController.Instance.enabled = true;
    }

    public void ShowResult()
    {
        //for normal game
        if (!RecordPlayer.Instance.IsPlaying)
        {
            //Stop record
            Recorder.Instance.StopRecord();
            //btnSaveRecord.SetActive(true);

            highscore.text = ScoreManager.Instance.GetHighestScore().ToString();
            ScoreManager.Instance.SubmitNewScore(Convert.ToInt32(scoreResult.text));
        }
        else
        {
            //btnSaveRecord.SetActive(false);
            highscore.text = "";
        }

        //stop background
        ParallaxBackground.Instance.setRunningState(false);

        scoreResult.text = scoreIngame.text;
        Main.SetActive(false);
        Ingame.SetActive(false);
        Result.SetActive(true);

        RecordPlayer.Instance.StopPlayingRecord();

        ShowInstruction(true);
    }

    public int GetScore()
    {
        return int.Parse(scoreIngame.text);
    }

    public void ShowInstruction(bool active)
    {
        Ingame.transform.GetChild(1).gameObject.SetActive(active);
        scoreIngame.text = 0 + "";
    }

    public void RestartButton()
    {
        Main.SetActive(true);
        Ingame.SetActive(false);
        Result.SetActive(false);

        UpdateCoinText();

        //run background
        ParallaxBackground.Instance.setRunningState(true);

        /* GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");

         foreach (var item in pipes)
         {
             Destroy(item);
         }*/
        SpawnerPipe.Instance.DestroyItem();

        BirdController.Instance.enabled = false;
        
    }

    public void ShopButtonOnClick()
    {
        Camera.main.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadScene("ShopItems", LoadSceneMode.Additive);
    }

    public void ShowRecordList(bool active)
    {
        RecordList.SetActive(active);
        contentList.LoadClip();

        btnFBLogin.interactable = btnRecordList.interactable = btnShop.interactable = !active;
    }

    public void ShowPanelLogout(bool active)
    {
        LogoutPanel.SetActive(active);

        btnFBLogin.interactable = btnRecordList.interactable = btnShop.interactable = btnStart.interactable = !active;
    }

    public void UpdateCoinText()
    {
        coin.text = PlayerPrefs.GetInt("totalCoins") + "";
    }
}
