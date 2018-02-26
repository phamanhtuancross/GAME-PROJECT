using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : MonoBehaviour {
    
    public Text KeyText;
    public Text GoldText;
    public GameObject KeyReward;
    public GameObject GoldReward;
    public GameObject Effect;
    public Sprite HomeIcon;

    public bool isShowing;

	void Update () {
		if(isShowing)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                GoldText.text = "x\t" + MissionManager.Instance.getGold();
                KeyText.text = "x\t1" ;                
                UIManager.Instance.PlayButtonAnimation("In");
                isShowing = false;
            }
        }
	}

    private IEnumerator ShowScore()
    {
        isShowing = true;
        yield return StartCoroutine(UIManager.Instance.UpdateScore(MissionManager.Instance.getGold() - 20, MissionManager.Instance.getGold(), GoldText));
        yield return StartCoroutine(UIManager.Instance.UpdateScore(0, 1, KeyText));
        isShowing = false;
        UIManager.Instance.PlayButtonAnimation("In");
    }

    public IEnumerator EmitParticle()
    {        
        GameObject go = Instantiate(Effect,UIManager.Instance.transform);
        go.GetComponent<ParticleSystem>().Play();

        yield return new WaitUntil(() => go.GetComponent<ParticleSystem>().isStopped);

        GoldText.text = "";
        KeyText.text = "";
        gameObject.SetActive(true);
        Destroy(go);
        GetComponent<Animator>().SetTrigger("Show");
        StartCoroutine(ShowScore());
    }
}
