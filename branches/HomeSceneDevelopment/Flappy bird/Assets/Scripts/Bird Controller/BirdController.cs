using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : Singleton<BirdController>
{

    //----------------------------------------variable prototype------------------------------
    // set Auto Mode
    private bool isAuto;
    private Vector3 posPipe;
    // end fix
    public float bounceForce;
    private Rigidbody2D myBody;
    private Animator anim;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip flyingClip, pingClip, DiedClip;
    private bool isAlive;
    public float touchDelay = 0.2f;
    private float touchCountdown;
    private int totalPoint;
    public bool flag = false;
    private Vector3 startPossition;
    private int totalCoins;
    private bool recordMode;
    private bool isPlayingRecord;
    //----------------------------------------end of variables prototype-------------------------

    //The BirdController Constructor-------------------------------------------------------------


    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }

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
    //set Auto Mode
    public bool IsAuto
    {
        get
        {
            return isAuto;
        }
        set
        {
            isAuto = value;
        }
    }
    //end fix

    public int TotalCoins
    {
        get
        {
            return totalCoins;
        }

        set
        {
            totalCoins = value;
        }
    }

    public bool IsPlayingRecord
    {
        get
        {
            return isPlayingRecord;
        }

        set
        {
            MyBody.constraints = RigidbodyConstraints2D.None;
            isPlayingRecord = value;
        }
    }

    public Rigidbody2D MyBody
    {
        get
        {
            return myBody;
        }

        set
        {
            myBody = value;
        }
    }

    public Animator Anim
    {
        get
        {
            return anim;
        }

        set
        {
            anim = value;
        }
    }

    public int TotalPoint
    {
        get
        {
            return totalPoint;
        }

        set
        {
        }
    }

    // Use this for initialization
    void Start()
    {

        this.TotalPoint = 0;
        this.TotalCoins = PlayerPrefs.GetInt("totalCoins");
    }


    private void Awake()
    {
        this.IsAlive = true;
        isAuto = false; // set auto mode
        this.MyBody = GetComponent<Rigidbody2D>();
        this.Anim = GetComponent<Animator>();
        // this.myBody.gameObject.e
        this.startPossition = MyBody.transform.position;
        //  GameObject.FindGameObjectWithTag("SpawnerPipe").SetActive(false);

    }

    private void Update()
    {
        touchCountdown -= Time.deltaTime;
        this.transform.position = new Vector3(startPossition.x, transform.position.y, 0);
        BirdRotation();
    }

    public void Controler()
    {
        _setAutoMode(); // auto mode
        BirdMovement();
        BirdRotation();
    }
    private void FixedUpdate()
    {
        _setAutoMode(); // auto mode
        BirdMovement();
    }

    public void FlapUp()
    {
        if (transform.position.y + this.GetComponent<SpriteRenderer>().bounds.size.y > Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y)
            return;

        // fix 
        if (this.isAuto)
        {
            this.isAuto = false;
            Destroy(GameObject.FindGameObjectWithTag("TimeCount"));
        }

        //end-fix
        touchCountdown = touchDelay;
        MyBody.velocity = new Vector2(MyBody.velocity.x, bounceForce);
        this.audioSource.PlayOneShot(flyingClip);
        touchCountdown = touchDelay;
        MyBody.velocity = new Vector2(MyBody.velocity.x, bounceForce);
        this.audioSource.PlayOneShot(flyingClip);
        touchCountdown = touchDelay;
        MyBody.velocity = new Vector2(MyBody.velocity.x, bounceForce);
        this.audioSource.PlayOneShot(flyingClip);
    }
    /*----------------------------------------------------------------------------------------------------
     *  void BirdMovement() : PUBLIC
     *  The function for move the bird when User tap to button
     *  The distance which bird can move  =  bounceForce * velocity of bird;
     */
    public void BirdMovement()
    {
        
        if (IsAlive)
        {
            if (Input.GetMouseButtonDown(0) && touchCountdown <= 0 && !isPlayingRecord)
            {
                //if (transform.position.y + this.GetComponent<SpriteRenderer>().bounds.size.y <= Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y)
                    FlapUp();
                //send fly record if in Record Mode
                if (recordMode)
                    Recorder.Instance.AddBirdFlyLog();
            }
        }
    }
    //---------------------------------------------end of function---------------------------------

    /*----------------------------------------------------------------------------------------------------
 *  void BirdRotation() : PRIVATE
 *  The function for move the bird when User tap to button
 *  The distance which bird can move  =  bounceForce * velocity of bird;
 */
    private void BirdRotation()
    {
        if (MyBody.velocity.y > 0)
        {
            float angel = 0;
            angel = Mathf.Lerp(0, 90, MyBody.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angel);
        }
        else
        {
            if (MyBody.velocity.y < 0)
            {
                float angel = 0;
                angel = Mathf.Lerp(0, -90, -MyBody.velocity.y / 7);
                transform.rotation = Quaternion.Euler(0, 0, angel);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    //---------------------------------------------end of function---------------------------------

    /*---------------------------------------------------------------------------------------------
     * void OnTriggerEnter2D(Collider2D target) : private
     * The function iplememtns whent the bird creosed a pipeHolder 
     * 
     */
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "PipeHolder" && isAlive)
        {
            audioSource.PlayOneShot(pingClip);
            this.totalPoint++;
            UIManager.Instance.scoreIngame.text = totalPoint + "";

            if(!IsPlayingRecord)
            {
                this.totalCoins++;
                UIManager.Instance.coin.text = totalCoins + "";
            }
            
            //reset point ->

        }
    }
    //-------------------------------------------end of function------------------------------------

    /*-----------------------------------------------------------------------------------------------
     * void OnCollisionEnter2D(Collision2D collision):private
     * The function for catche the event for the bird and pipe and ground
     */
    private void OnCollisionEnter2D(Collision2D target)
    {

        if ((target.gameObject.tag == "Pipe" || target.gameObject.tag == "Ground") && IsAlive == true)
        {
            flag = true;
            audioSource.PlayOneShot(DiedClip);
            IsAlive = false;
            this.Anim.SetBool("Died", true);
            Anim.SetBool("Flapping", false);
            if (!IsPlayingRecord)
            {
                PlayerPrefs.SetInt("totalCoins", totalCoins);
                this.totalCoins = PlayerPrefs.GetInt("totalCoins");
                this.TotalPoint = 0;
            }
            this.totalPoint = 0;
        }
    }
    //------------------------------------------end of function---------------------------------------


    /*-------------------------------------------------------------------------------------------------
     * void OnButtonRestartClick() : public 
     * The function when the buton restart click 
     * -> reset the state for  bird 
     * -> state inlude : posstion , animator , state
     */
    public void OnButtonRestartClick()
    {
        Debug.Log("CLICKED");
        this.MyBody.transform.position = startPossition;
        this.isAlive = true;
        //fix
        if (isAuto)
        {
            isAuto = false;
            MyBody.gravityScale = 1;
        }
        SpawnerPipe.Instance.IsMangetMode = false;
        //fix
        Anim.SetBool("Flapping", true);//--change the animation for bird animator
        this.Anim.SetBool("Died", false);
        MyBody.velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        MyBody.constraints = RigidbodyConstraints2D.FreezeAll;
        isPlayingRecord = false;


        this.totalPoint = 0;
        UIManager.Instance.scoreIngame.text =   "" + TotalPoint;
    }

    //--------------------------------------------------end of function--------------------------------
    //Auto Mode
    private void _setAutoMode()
    {
        if (isAuto)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            MyBody.velocity = Vector2.zero;
            _autoMove();
        }
        else
        {
            MyBody.gravityScale = 1;
        }
    }
    public void _autoMove()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipeHolder");
        foreach (var item in pipes)
        {
            if (item.transform.position.x - gameObject.transform.position.x + 0.8f > 0)
            {
                Vector3 temp = gameObject.transform.position;
                float angel = 0;
                if (temp.y - item.transform.position.y > 0)
                {
                    MyBody.gravityScale = -1;
                    MyBody.velocity = new Vector2(MyBody.velocity.x, -1);
                    angel = Mathf.Lerp(0, -90, MyBody.velocity.y / 7);
                    temp.y -= PipeHolder.instance.speed * 2 * Time.deltaTime;
                }
                else if (temp.y - item.transform.position.y < -0.2f)
                {
                    MyBody.gravityScale = 1;
                    MyBody.velocity = new Vector2(MyBody.velocity.x, 1);
                    angel = Mathf.Lerp(0, 90, MyBody.velocity.y / 7);
                    temp.y += PipeHolder.instance.speed * 2 * Time.deltaTime;
                }
                else
                {
                    MyBody.gravityScale = 0;
                }
                transform.rotation = Quaternion.Euler(0, 0, angel);
                gameObject.transform.position = temp;
                break;
            }
        }
    }
}
