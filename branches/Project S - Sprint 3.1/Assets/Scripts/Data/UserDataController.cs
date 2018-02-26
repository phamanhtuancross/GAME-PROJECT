using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserDataController : MonoBehaviour
{

    public Text textHeartTime;
    public Text textHeartNumber;
    public Text textGoldNumber;
    public Text textKeyNumber;

    public Button startLevelButton;

    private void Start()
    {
        InvokeRepeating("UpdateHeart", 0f, 1f);
        InvokeRepeating("UpdateGold", 0f, 1f);
        InvokeRepeating("UpdateKey", 0f, 1f);
        InvokeRepeating("UpdateItems", 0f, 1f);
        textHeartNumber.text = UserData.GetHeart().ToString();
        textGoldNumber.text = UserData.GetGold().ToString();
        textKeyNumber.text = UserData.GetKey().ToString();


        //Set volume on start
        AudioMixerControl.Instance.setMusicLvl(UserData.GetMusicVolume());
        AudioMixerControl.Instance.setSFXLvl(UserData.GetSFXVolume());
    }

    private void UpdateHeart()
    {
        System.TimeSpan infHeartTimeLeft = UserData.GetInfHeartTimeLeft();
        if (infHeartTimeLeft > System.TimeSpan.Zero)
        {
            textHeartNumber.text = "Inf";
            textHeartTime.text = (int)infHeartTimeLeft.TotalMinutes + ":" + infHeartTimeLeft.Seconds;
            return;
        }

        int updatedHeart = UserData.GetHeart();

        //update start level button
        if (updatedHeart <= 0)
            startLevelButton.enabled = false;
        else
            startLevelButton.enabled = true;

        //update heart text
        textHeartNumber.text = updatedHeart.ToString();
        if (updatedHeart >= 5) //full heart
        {
            textHeartTime.text = "FULL";
            return;
        }

        System.TimeSpan timePassed = System.DateTime.Now.Subtract(UserData.GetHeartTime());   //get time between now and the last heart time

        if(timePassed.TotalMinutes > 30d)   //if need to add heart
        {
            double numHeart = timePassed.TotalMinutes / 30d;    //number of Heart to add

            while(numHeart > 0 && UserData.GetHeart() <= 5)
            {
                //adding
                UserData.AddHeart(true);
                numHeart--;
            }

            //update heart text
            textHeartNumber.text = UserData.GetHeart().ToString();
            if (int.Parse(textHeartNumber.text) >= 5) //full heart
            {
                textHeartTime.text = "FULL";
                return;
            }
            textHeartTime.text = "30:00";
        }
        else
        {
            //update heart text
            textHeartTime.text = ( 29 - timePassed.Minutes).ToString() + ":" + (60 - timePassed.Seconds).ToString();
        }
    }

    private void UpdateGold()
    {
        textGoldNumber.text = UserData.GetGold().ToString();
    }

    private void UpdateKey()
    {
        textKeyNumber.text = UserData.GetKey().ToString();
    }

    private void UpdateItems()
    {
        foreach(ItemsType type in System.Enum.GetValues(typeof(ItemsType)))
        {
            double hoursPassed = System.DateTime.Now.Subtract(UserData.GetItemTime(type)).TotalHours;
            DebugUtil.debug(type.ToString() + " count: " + UserData.GetItemCount(type));
            if (hoursPassed >= 24d)
            {
                UserData.AddItem(type, 1);
            }
        }
    }

    public void CheatAddHeart(int count)
    {
        while(count > 0)
        {
            UserData.AddHeart(false);
            count--;
        }
    }
}
