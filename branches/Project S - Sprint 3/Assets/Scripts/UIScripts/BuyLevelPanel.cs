using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyLevelPanel : MonoBehaviour {

    private int cost;

    public int Cost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;
            CostText.text = cost.ToString();
        }
    }

    public Text CostText;
    public Animator anim;
    public Button BuyButton;
    public GameObject RetryButton, HomeButton;
    public GameObject Message;

    public void Show()
    {
        RetryButton.SetActive(false);
        HomeButton.SetActive(false);

        Mission currrentMission = MissionManager.Instance.CurrentMission;
        Cost = (int)currrentMission.difficult * 1000;        

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BuyPanelShow"))
            anim.SetTrigger("Show");
    }

    public void Close()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BuyPanelHide"))
            anim.SetTrigger("Hide");

        RetryButton.SetActive(true);
        HomeButton.SetActive(true);
    }

    public void BuyLevel()
    {
        if (UserData.GetGold() >= cost)
        {
            MissionManager.Instance.UpdateMissionPass();
            UserData.SubtractGold(cost);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BuyPanelHide"))
                anim.SetTrigger("Hide");
            UIManager.Instance.UpdateButtonInfo();
            UIManager.Instance.PlayButtonAnimation("In");
            PlayerPrefs.SetInt(DataKeys.LEVEL_FAILED_COUNT, 0);
        }
        else
        {
            GameObject go = Instantiate(Message, UIManager.Instance.transform);
            Destroy(go, 1f);
        }
    }
}
