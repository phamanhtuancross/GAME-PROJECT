using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemPanel : MonoBehaviour
{
    private int cost;
    private int quanlity;

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

    public int Quanlity
    {
        get
        {
            return quanlity;
        }
        set
        {
            quanlity = value;
            QuanlityText.text = quanlity.ToString();
        }
    }
    
    private ItemsType Type;
    public Sprite[] ItemIcons;
    public Image Icon;
    public Text CostText;
    public Text QuanlityText;
    public Animator anim;
    public Button IncreaseButton;
    public Button DecreaseButton;
    public Button BuyItemButton;
    public GameObject Message;
    
    public void Show(ItemHandler item)
    {
        Type = item.Type;
        Icon.sprite = ItemIcons[(int)Type];
        Quanlity = 1;
        Cost = 1000;

        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("BuyPanelShow"))
            anim.SetTrigger("Show");

        //if (cost > UserData.GetGold())
        //    BuyItemButton.interactable = false;
        //else
        //    BuyItemButton.interactable = true;

        CheckToIncrease();
        CheckToDecrease();
    }

    public void Close()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BuyPanelHide"))
            anim.SetTrigger("Hide");
    }

    public void IncreaseQuanlity()
    {
        DecreaseButton.interactable = true;
        Quanlity++;

        Cost = Quanlity * 1000;
        CheckToIncrease();
        CheckToDecrease();
    }

    public void DecreaseQuanlity()
    {
        if (quanlity == 1)
        {
            return;
        }

        Quanlity--;

        Cost = Quanlity * 1000;
        CheckToIncrease();
        CheckToDecrease();
    }

    public void BuyButton()
    {
        if (UserData.GetGold() >= cost)
        {
            UserData.AddItem(Type, quanlity);
            UserData.SubtractGold(cost);

            this.Close();
        }
        else
        {
            GameObject go = Instantiate(Message, UIManager.Instance.transform);
            Destroy(go, 1f);
        }
    }

    public void CheckToIncrease()
    {
        if (UserData.GetGold() <= (cost + 1000))
        {
            IncreaseButton.gameObject.SetActive(false);
        }
        else
        {
            IncreaseButton.gameObject.SetActive(true);
        }
    }

    public void CheckToDecrease()
    {
        if (quanlity == 1)
        {
            DecreaseButton.gameObject.SetActive(false);
        }
        else
        {
            DecreaseButton.gameObject.SetActive(true);
        }
    }
}
