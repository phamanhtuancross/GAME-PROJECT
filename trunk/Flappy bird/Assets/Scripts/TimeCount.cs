using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : Singleton<TimeCount> {

	private float time;

    private Image Timer;
    private float time2;
    [SerializeField]
    GameObject magnetState;


    void Start()
    {
        _setStart();
        Timer = gameObject.GetComponent<Image>();
    }

    void _setStart()
    {
        time = 8f;
        time2 = 8;
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = BirdController.Instance.transform.position;
        gameObject.transform.localScale = new Vector3(2.6f, 2.6f, 1);
        GameObject[] mag = GameObject.FindGameObjectsWithTag("TimeCount");
        if (mag.Length > 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("MagnetState"));
            Destroy(GameObject.FindGameObjectWithTag("TimeCount"));
        }
        if (SpawnerPipe.Instance.IsMangetMode)
        {
            GameObject newTimeCount = Instantiate(magnetState, magnetState.transform.position, Quaternion.identity);
            newTimeCount.transform.SetParent(gameObject.transform);
        }
    }

	void Update () {
        this.gameObject.transform.position = BirdController.Instance.transform.position;
        if (time2 > 0)
        {
            Timer.fillAmount -= 1.0f / time * Time.deltaTime;
            time2 -= Time.deltaTime;
        } else
        {
            if (BirdController.Instance.IsAuto)
                BirdController.Instance.IsAuto = false;
            else
                SpawnerPipe.Instance.IsMangetMode = false;
            Destroy(gameObject);
        }
    }
}
