using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct NumberSprite
{
    public int number;
    public Sprite sprites;
}
[System.Serializable]
public class ObjectiveType
{
    public Image Icon;
    public GameObject Type;

    public void Setup(Sprite _icon)
    {
            Icon.sprite = _icon;
    }



}

public class QuestManager : Singleton<QuestManager> {
    [SerializeField]
    private NumberSprite[] numbers = new NumberSprite[9];

    public Transform PanelQuest;
    public GameObject QuestInfo;
    public List<ObjectiveType> Type = new List<ObjectiveType>();

    private int ChooseId;
    private Quest questComplete = null;

    private List<int> currentQuest = new List<int>();
    [Header("Panel")]
    public GameObject EnoughKeyPanel;
    public GameObject ChooseTypeBuilt;
    public GameObject DarkState;

    public List<int> CurrentQuest
    {
        set
        {
            if (currentQuest != value)
            {
                currentQuest = value;
                setQuestUI(value);
            }
            
        }
    }
    private void setQuestUI(List<int> quests)
    {
        GameObject[] lQuestPanel = GameObject.FindGameObjectsWithTag("Quest");
        for(int i = 0; i < lQuestPanel.Length; i++)
        {
            Destroy(lQuestPanel[i]);
        }

        for(int i = 0; i < quests.Count; i++)
        {
            Quest temp;
            QuestController.Instance.LQuest.TryGetValue(quests[i],out temp);
            if(temp != null)
            {
                GameObject newQuest = Instantiate(QuestInfo) as GameObject;
                newQuest.transform.GetChild(2).GetComponent<Image>().sprite = temp.QuestSpriteIcon;
                newQuest.transform.GetChild(1).GetComponent<Text>().text = temp.Description;
                newQuest.transform.GetChild(0).Find("Number").GetComponent<Image>().sprite = getSpriteNumber(temp.amountKey);
                newQuest.transform.SetParent(PanelQuest, false);
                newQuest.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DoItButton(i));
            }
        }
    }

    private void Awake()
    {
        CurrentQuest = UserData.getCurrentQuest();
        questComplete = null;
    }

    public Sprite getSpriteNumber(int number)
    {
        for(int i = 0;i < numbers.Length; i++)
        {
            if (number == numbers[i].number)
            {
                return numbers[i].sprites;
            }
        }
        return null;
    }

    public void DoItButton(int questPanelID)
    {
        questComplete = null;
        QuestController.Instance.LQuest.TryGetValue(currentQuest[questPanelID - 1], out questComplete);
        if (questComplete != null) {
            if (UserData.GetKey() >= questComplete.amountKey)
            {
                QuestClear();
            }
            else
            {
                QuestFail();
            }
        }
    }
    public void QuestClear()
    {
        ChooseId = 0;
        ChooseTypeBuilt.SetActive(true);
        UIManager.Instance.QuestInfoPanel.GetComponent<Animator>().SetTrigger("Out");
        UIManager.Instance.CallDarkStateBackground(false);
        for (int i = 0; i < Type.Count; i++)
        {
            Type[i].Setup(questComplete.listType[i]);
        }
        ChooseTypeBuilt.SetActive(true);
        ChooseTypeBuilt.transform.GetChild(0);
        AnimationChooseTypePanel("In");
    }
    public void QuestFail()
    {
        EnoughKeyPanel.SetActive(true);
        EnoughKeyPanel.GetComponent<Animator>().SetTrigger("+");
        this.CallDarkState(true);
    }
    public IEnumerator StartBuilt(Quest quest)
    {
        Image FaddingImage = UIManager.Instance.FaddingImage;
        FaddingImage.enabled = true;
        while (FaddingImage.color.a < 0.9f)
        {
            FaddingImage.color = Color.Lerp(FaddingImage.color, Color.black, .1f);
            yield return new WaitForSeconds(.01f);
        }
        FaddingImage.color = Color.black;

        yield return new WaitForSeconds(.5f);

        while (FaddingImage.color.a > 0.1f)
        {
            FaddingImage.color = Color.Lerp(FaddingImage.color, Color.clear, .1f);
            yield return new WaitForSeconds(.01f);
        }
        FaddingImage.color = Color.clear;
        FaddingImage.enabled = false;

    }
    private void AnimationChooseTypePanel(string anim)
    {
        GameObject ChooseContainer =  ChooseTypeBuilt.transform.GetChild(0).gameObject;
        ChooseContainer.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger(anim);
        ChooseContainer.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger(anim);
        ChooseContainer.transform.GetChild(2).gameObject.GetComponent<Animator>().SetTrigger(anim);
    }

    public void CloseChooseButton()
    {
        AnimationChooseTypePanel("Out");
        ChooseTypeBuilt.SetActive(false);
        UIManager.Instance.PlayButtonAnimation("In");
    }
    public void OkButton()
    {
        AnimationChooseTypePanel("Out");
        ChooseTypeBuilt.SetActive(false);
        UIManager.Instance.PlayButtonAnimation("In");
        DebugUtil.debugYellow(ChooseId.ToString());
        for(int i = 0; i <currentQuest.Count; i++)
        {
            if (currentQuest[i] == questComplete.STT)
            {
                currentQuest.RemoveAt(i);
                for(int j = 0; j < questComplete.nextQuest.Length; j++)
                {
                    currentQuest.Add(questComplete.nextQuest[j]);
                }
            }
        }
        UserData.updateCurrentQuest(currentQuest);
        setQuestUI(currentQuest);
        UserData.SubtractKey(questComplete.amountKey);
    }

    public void OnChoose(int type)
    {
        if (ChooseId != type) {
            switch (type)
            {

            }
        }
    }

    Color unChoose = new Color32(60, 83, 123, 255);
    Color Choose = new Color32(212, 218, 228, 255);
    public void ChangeTabQuestInfo(int tab)
    {
        GameObject container = UIManager.Instance.QuestInfoPanel.transform.GetChild(0).gameObject;

        switch (tab)
        {
            case 1: // TO DO
                // Arrow
                container.transform.Find("Todo").GetChild(0).GetChild(0).gameObject.SetActive(true);
                container.transform.Find("Inbox").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Newsfeed").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("News").GetChild(0).GetChild(0).gameObject.SetActive(false);
                // Change Color
                container.transform.Find("Todo").GetChild(0).GetComponent<Image>().color = Choose;
                container.transform.Find("Inbox").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Newsfeed").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("News").GetChild(0).GetComponent<Image>().color = unChoose;
                break;
            case 2: // NEWSFEED
                container.transform.Find("Todo").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Inbox").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Newsfeed").GetChild(0).GetChild(0).gameObject.SetActive(true);
                container.transform.Find("News").GetChild(0).GetChild(0).gameObject.SetActive(false);

                container.transform.Find("Todo").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Inbox").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Newsfeed").GetChild(0).GetComponent<Image>().color = Choose;
                container.transform.Find("News").GetChild(0).GetComponent<Image>().color = unChoose;
                break;
            case 3: //INBOX
                container.transform.Find("Todo").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Inbox").GetChild(0).GetChild(0).gameObject.SetActive(true);
                container.transform.Find("Newsfeed").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("News").GetChild(0).GetChild(0).gameObject.SetActive(false);

                container.transform.Find("Todo").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Inbox").GetChild(0).GetComponent<Image>().color = Choose;
                container.transform.Find("Newsfeed").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("News").GetChild(0).GetComponent<Image>().color = unChoose;
                break;
            case 4: // NEWS
                container.transform.Find("Todo").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Inbox").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("Newsfeed").GetChild(0).GetChild(0).gameObject.SetActive(false);
                container.transform.Find("News").GetChild(0).GetChild(0).gameObject.SetActive(true);

                container.transform.Find("Todo").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Inbox").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("Newsfeed").GetChild(0).GetComponent<Image>().color = unChoose;
                container.transform.Find("News").GetChild(0).GetComponent<Image>().color = Choose;
                break;
            default:
                DebugUtil.debugError("No tab");
                break;
        }
    }
    public void CallDarkState(bool isOpen)
    {
        DarkState.SetActive(isOpen);
    }
}
