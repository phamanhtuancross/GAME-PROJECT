using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPipe : Singleton<SpawnerPipe> {
    public GameObject PipeContainer;
	public GameObject TokenContainer;
    [SerializeField]
    private Sprite Magnet;

    [SerializeField]
    private Sprite CrazyBird;

    [SerializeField]
    private GameObject pipeHolder;
    private Vector3 spawnPos;

    private bool recordMode;
	 [SerializeField]
    private GameObject token;
    [SerializeField]
    private GameObject item;
    private Vector3 posPipe;
    private bool isToken;
    private float timeItem;
    private float timeHave;
    private float countdownPipeSpawn;
    private float countdownTokenSpawn;
    private bool isMangetMode;
    private GameObject newToken;

    public bool RecordMode
    {
        get
        {
            return recordMode;
        }

        set
        {
            recordMode = value;
        }
    }

    private bool isPlaying;

	private bool isPlayingRecord;

    public bool IsPlayingRecord
    {
        get
        {
            return isPlayingRecord;
        }

        set
        {
            isPlayingRecord = value;
        }
    }
	public bool IsMangetMode
    {
        get
        {
            return isMangetMode;
        }

        set
        {
            isMangetMode = value;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }

        set
        {
            isPlaying = value;
        }
    }

    // Use this for initialization
    void Start () {
         Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 50, 0, 0));
        temp.z = 0;
        spawnPos = temp;
        timeItem = 0;
        countdownPipeSpawn = 0;
        countdownTokenSpawn = 0;
        this.isToken = true;
        timeHave = 2.0f;
        isMangetMode = false; // fix false;
    }

    private void FixedUpdate()
    {
        timeItem += Time.fixedDeltaTime;
        countdownPipeSpawn -= Time.fixedDeltaTime;
        countdownTokenSpawn -= Time.fixedDeltaTime;
        if (timeItem >= timeHave)
        {
            this.isToken = false;
            timeItem = 0;
        }

        if (isPlayingRecord || !IsPlaying)
            return;

        if (countdownPipeSpawn <= 0)
        {
            spawner();
            countdownPipeSpawn = 3f;
        }
        if (countdownTokenSpawn <= 0)
        {
            crtToken();
            countdownTokenSpawn = 3f;
        }
    }

    void spawner()
    {
        Vector3 temp = spawnPos;
        temp.y = Random.Range(-2.5f, 2.78f);
        posPipe = temp;
        GameObject newPipe = Instantiate(pipeHolder, temp, Quaternion.identity);
        newPipe.transform.SetParent(PipeContainer.transform);
        
     	  //send record if in RecordMode
        if (recordMode)
            Recorder.Instance.AddPipeSpawnLog(temp.y);    
    }
	
	  void crtToken()
    {
        Vector3 temp = spawnPos;
        temp.y = Random.Range(-2.5f, 2.78f);
        temp.x += PipeHolder.instance.speed;
        if (isToken)
        {
            temp.x--;
            int numToken = Random.Range(1, 4);
            for (int i = 0; i <= numToken; i++)
            {
                if (i < 2)
                {
                    temp.x += i;
                    newToken = Instantiate(token, temp, Quaternion.identity);
                }
                else
                {
                    if (i == 2)
                    {
                        temp.y += 1;
                    }
                    else
                    {
                        temp.x -= 1;
                    }
                    newToken = Instantiate(token, temp, Quaternion.identity);
                  
                }
                newToken.transform.SetParent(TokenContainer.transform);
            }

            //send record if in RecordMode
            if (recordMode)
                Recorder.Instance.AddTokenLog(temp.y, numToken);
        } else
        {
            newToken = Instantiate(item, temp, Quaternion.identity);
            
            timeHave += 5.0f;
            this.isToken = true;
            int type = Random.Range(0,2);
            if (type != 0)
                setSpriteMagnet(newToken);
            else 
                setSpriteCrazyBird(newToken);

            newToken.transform.SetParent(TokenContainer.transform);

            //send record if in RecordMode
            if (recordMode)
                Recorder.Instance.AddItemLog(temp.y, type);
        }
        
    }

	
	public void SpawnPipe(float PosY)
	{
		Vector3 temp = spawnPos;
        temp.y = PosY;
        GameObject newPipe = Instantiate(pipeHolder, temp, Quaternion.identity);
        newPipe.transform.SetParent(PipeContainer.transform);
	}

    public void SpawnItem(float PosY, int type)
    {
        Vector3 temp = spawnPos;
        temp.y = PosY;
        temp.x += PipeHolder.instance.speed;

        newToken = Instantiate(item, temp, Quaternion.identity);

        if (type != 0)
            setSpriteMagnet(newToken);
        else
            setSpriteCrazyBird(newToken);

        newToken.transform.SetParent(TokenContainer.transform);
    }

    public void SpawnToken(float PosY, int Count)
    {
        Vector3 temp = spawnPos;
        temp.y = PosY;
        temp.x += PipeHolder.instance.speed;

        temp.x--;
        for (int i = 0; i <= Count; i++)
        {
            if (i < 2)
            {
                temp.x += i;
                newToken = Instantiate(token, temp, Quaternion.identity);
            }
            else
            {
                if (i == 2)
                {
                    temp.y += 1;
                }
                else
                {
                    temp.x -= 1;
                }
                newToken = Instantiate(token, temp, Quaternion.identity);

            }
            newToken.transform.SetParent(TokenContainer.transform);
        }
    }

	 private void setSpriteCrazyBird(GameObject bird)
    {
        bird.GetComponent<SpriteRenderer>().sprite = CrazyBird;
        bird.transform.localScale = bird.transform.localScale / 3;
        bird.gameObject.tag = "CrazyBird";
    }

    public void setSpriteMagnet(GameObject magnet)
    {
        magnet.GetComponent<SpriteRenderer>().sprite = Magnet;
        magnet.transform.localScale = magnet.transform.localScale / 2;
        magnet.gameObject.tag = "Magnet";
    }
    public void DestroyItem()
    {
        string[] destroyTag =
        {
            "PipeHolder",
            "Token",
            "Magnet",
            "CrazyBird",
            "TimeCount",
            "MagnetState"

        };
        foreach(string tag in destroyTag)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject gameObj in gameObjects)
            {
                Destroy(gameObj);
            }
        }
    }
}
