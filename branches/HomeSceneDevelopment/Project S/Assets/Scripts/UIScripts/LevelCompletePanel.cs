using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : MonoBehaviour {

    public Text ScoreText;
    public Text KeyText;
    public Text GoldText;
    public GameObject KeyReward;
    public GameObject GoldReward;
    public GameObject Effect;

    public bool isShowing;

	void Update () {
		if(isShowing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                ScoreText.text = UIManager.Instance.Score.ToString();
                GoldText.text = MissionManager.Instance.getGold().ToString();
                KeyText.text = 1+ "";
                isShowing = false;
            }
        }
	}

    private IEnumerator ShowScore()
    {
        isShowing = true;
        ScoreText.text = GoldText.text = KeyText.text = "";
        yield return StartCoroutine(UIManager.Instance.UpdateScore(UIManager.Instance.Score - 20, UIManager.Instance.Score, ScoreText));
        yield return StartCoroutine(UIManager.Instance.UpdateScore(MissionManager.Instance.getGold() - 20, MissionManager.Instance.getGold(), GoldText));
        StartCoroutine(UIManager.Instance.UpdateScore(0, 1, KeyText));
        isShowing = false;
    }

    public void EmitParticle()
    {        
        GameObject go = Instantiate(Effect, transform);
        go.GetComponent<ParticleSystem>().Play();

        StartCoroutine(ShowScore());
    }
}
