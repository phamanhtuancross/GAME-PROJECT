using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDailyBonus : MonoBehaviour
{
    [Header("Main Panel")]
    [Tooltip("Place in order from chest 1 to 7")]
    public UIDailyChest[] Chest;
    [Tooltip("Place in order from week 1 to 4")]
    public WeeklyBonusData[] Weeks;
    public Button ClaimButton;
    public GameObject CloseButton;
    public Text WeekText;

    [Space(15)]

    [Header("Message Panel")]
    public GameObject MessagePanel;
    public Image BonusImage;
    public Text BonusText;

    private bool isCanClaim;
    private int currentWeek;
    private int currentDay;

    private void Start()
    {
        InvokeRepeating("UpdateTimeLeft", 0, 1f);
    }

    public void ShowDailyBonus()
    {
        MessagePanel.SetActive(false);
        Refresh();
    }

    public void DeactivePanel()
    {
        //this.gameObject.SetActive(false);
    }

    public void UpdateTimeLeft()
    {
        if (isCanClaim) //no need to update time
            return;
        System.TimeSpan timePassed = UserData.GetDailyBonusTimePassed();
        isCanClaim = timePassed.TotalHours >= 24d;  //check this frame
        if (isCanClaim)
        {
            Refresh();
            return;
        }

        //update time left count down
        System.TimeSpan timeLeft = (new System.TimeSpan(24, 0, 0)) - timePassed;
        ClaimButton.GetComponentInChildren<Text>().text = timeLeft.Hours + ":" + timeLeft.Minutes + ":" + timeLeft.Seconds;
    }

    public void Refresh()
    {
        isCanClaim = UserData.GetDailyBonusTimePassed().TotalHours >= 24d;
        currentWeek = UserData.GetDailyBonusWeek();
        currentDay = UserData.GetDailyBonusDay();

        if (isCanClaim)
        {
            ClaimButton.interactable = true;
            CloseButton.SetActive(false);
            ClaimButton.GetComponentInChildren<Text>().text = "CLAIM";
        }
        else
        {
            ClaimButton.interactable = false;
            CloseButton.SetActive(true);
        }

        WeekText.text = "WEEK " + UserData.GetDailyBonusWeek();
        Chest[currentDay - 1].ChestText.text = "TODAY";

        //Disable checkmarks
        for (int day = currentDay + 1; day <= 7; day++)
        {
            Chest[day - 1].CheckMark.SetActive(false);
        }
        if (isCanClaim)
            Chest[currentDay - 1].CheckMark.SetActive(false);

        //show bonus claimed
        for (int day = currentDay - 1; day > 0; day--)
        {
            Chest[day - 1].Chest.SetActive(false);
            Chest[day - 1].Bonus.SetActive(true);
            Chest[day - 1].Bonus.GetComponent<Image>().sprite = Weeks[currentWeek - 1].weekBonus[day - 1].bonusSprite;
        }
        if (!isCanClaim)
        {
            Chest[currentDay - 1].Chest.SetActive(false);
            Chest[currentDay - 1].Bonus.SetActive(true);
            Chest[currentDay - 1].Bonus.GetComponent<Image>().sprite = Weeks[currentWeek - 1].weekBonus[currentDay - 1].bonusSprite;
        }
    }

    public void ClaimClick()
    {
        DailyBonusData data = Weeks[currentWeek - 1].weekBonus[currentDay - 1];

        //claim
        UserData.ClaimDailyBonus();
        switch (data.type)
        {
            case BonusType.LIFE:
                System.TimeSpan timeINF = new System.TimeSpan(0, data.count, 0);
                UserData.AddInfHeart(timeINF);
                break;
            case BonusType.GOLD:
                UserData.AddGold(data.count);
                break;
            case BonusType.KEY:
                break;
            case BonusType.ITEM_BRUSH:
                UserData.AddItem(ItemsType.BRUSH_ITEM, data.count);
                break;
            case BonusType.ITEM_HAMMER:
                UserData.AddItem(ItemsType.HAMMER_ITEM, data.count);
                break;
            case BonusType.ITEM_CLOCK:
                UserData.AddItem(ItemsType.CLOCK_ITEM, data.count);
                break;
            default:
                break;
        }

        //update DailyBonus panel
        ClaimButton.interactable = false;
        Chest[currentDay - 1].Chest.SetActive(false);
        Chest[currentDay - 1].Bonus.SetActive(true);
        Chest[currentDay - 1].Bonus.GetComponent<Image>().sprite = data.bonusSprite;
        Chest[currentDay - 1].CheckMark.SetActive(true);
        CloseButton.SetActive(true);

        //update Message panel
        MessagePanel.SetActive(true);
        BonusImage.sprite = data.bonusSprite;
        BonusText.text = data.title;
    }
}
