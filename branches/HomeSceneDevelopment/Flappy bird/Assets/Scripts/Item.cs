using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour {

    private float speedRotate;
    private float widthCollider;
    private int coin;
    private float magnetMax;
    private float magnetMin;
    private bool isGo;

    [SerializeField]
    private GameObject TimeCountDown;

    void Awake () {
        speedRotate = 4;
        isGo = false;
        if (gameObject.tag != "Token")
        {
            transform.localScale = transform.localScale / 3;
           
        }
        widthCollider = _getWidthCollider();
    }

	void Update () {
        if (gameObject.tag != "Token")
            transform.Rotate(0, 0, speedRotate + Time.deltaTime);
        if (isGo)
        {
            Vector3 temp2 = gameObject.transform.position;
            if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x > 0.2)
            {
                temp2.x -= PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            else if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x < -0.2)
            {
                temp2.x += PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            if (temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y > 0.2)
            {
                temp2.y -= PipeHolder.instance.speed * 3 * Time.deltaTime; ;
            }
            else if (temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y < -0.2)
            {
                temp2.y += PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            gameObject.transform.position = temp2;
            /*magnetMax = 4;
            magnetMin = 0.2f;
            Vector3 temp2 = transform.position;
            if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x <= magnetMax
                     && temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x >= magnetMin
                     && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y <= magnetMax
                     && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y >= magnetMin)
            {
                temp2.x -= PipeHolder.instance.speed * 3 * Time.deltaTime;
                temp2.y -= PipeHolder.instance.speed * 3 * Time.deltaTime;

            }
            else if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x <= magnetMax
                && temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x >= magnetMin
                && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y >= -magnetMax
                && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y <= -magnetMin)
            {
                temp2.x -= PipeHolder.instance.speed * 3 * Time.deltaTime;
                temp2.y += PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            else if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x >= -magnetMax
                  && temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x <= -magnetMin
                  && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y <= magnetMax
                  && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y >= magnetMin)
            {
                temp2.x += PipeHolder.instance.speed * 3 * Time.deltaTime;
                temp2.y -= PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            else if (temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x >= -magnetMax
                  && temp2.x - GameObject.FindGameObjectWithTag("Player").transform.position.x <= -magnetMin
                  && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y >= -magnetMax
                  && temp2.y - GameObject.FindGameObjectWithTag("Player").transform.position.y <= -magnetMin)
            {
                temp2.x += PipeHolder.instance.speed * 3 * Time.deltaTime;
                temp2.y += PipeHolder.instance.speed * 3 * Time.deltaTime;
            }
            transform.position = temp2;*/
        }  
        _tokenMovement();
    }

    float _getWidthCollider()
    {
        BoxCollider2D collider = (BoxCollider2D)this.gameObject.GetComponent<BoxCollider2D>();
        float unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;
        return collider.size.x / unitsPerPixel;
    }

    private void _tokenMovement()
    {
        if (BirdController.Instance.IsAlive)
        {
            Vector3 temp = transform.position;
            temp.x -= PipeHolder.instance.speed * Time.deltaTime;
            transform.position = temp;
        }


        Vector3 temp2 = Camera.main.WorldToScreenPoint(transform.position);
        if (temp2.x + widthCollider / 2 < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "Token")
        {
            if (!BirdController.Instance.IsPlayingRecord)
            {
                BirdController.Instance.TotalCoins++;
                UIManager.Instance.coin.text = "" + BirdController.Instance.TotalCoins;
            }
            Destroy(gameObject);
        }
        else if (this.gameObject.tag == "Magnet" && collision.gameObject.tag == "Player")
        {
            SpawnerPipe.Instance.IsMangetMode = true;
            GameObject newTimeCount = Instantiate(TimeCountDown, TimeCountDown.transform.position, Quaternion.identity);
            newTimeCount.transform.SetParent(GameObject.FindGameObjectWithTag("TimeCountContainer").transform);
            Destroy(gameObject);
        }
        else if (this.gameObject.tag == "CrazyBird" && collision.gameObject.tag == "Player")
        {
            GameObject newTimeCount = Instantiate(TimeCountDown, TimeCountDown.transform.position, Quaternion.identity);
            newTimeCount.transform.SetParent(GameObject.FindGameObjectWithTag("TimeCountContainer").transform);
            BirdController.Instance.IsAuto = true;
            Destroy(gameObject);
        }
        if (this.gameObject.tag == "Token" && collision.gameObject.tag == "MagnetState")
            isGo = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "PipeHolder")
            Destroy(gameObject);
    }
}
