using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetState : MonoBehaviour {
    private float time;

    private Image Timer;
    private float time2;
    private float speedRotate;
    void Start()
    {
        _setStart();
        Timer = gameObject.GetComponent<Image>();
    }

    void _setStart()
    {
        time = 8f;
        time2 = 8;
        speedRotate = 4;
        
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = BirdController.Instance.transform.position;
        gameObject.transform.localScale = new Vector3(3.8f, 3.8f, 1);
    }

    void Update()
    {
        this.gameObject.transform.position = BirdController.Instance.transform.position;
        if (time2 > 0)
        {
            transform.Rotate(0, 0, -speedRotate + Time.deltaTime);
            time2 -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
