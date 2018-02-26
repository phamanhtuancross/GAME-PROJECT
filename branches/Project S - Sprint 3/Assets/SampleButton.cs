using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleButton : MonoBehaviour
{

    public Button buttonComponent;
    public Text nameItemLable;
    public Text priceOfItemLable;
    public Image iconItemImage;

    private ItemInStoreInfor item;
    private ShopScrollList scrollList;


    private void Start()
    {
        buttonComponent.onClick.AddListener(HandleButtonOnClick);
    }
    public void SetUpForSampleButton(ItemInStoreInfor currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        nameItemLable.text = item.nameOfItem;
        priceOfItemLable.text = item.price + "";
        iconItemImage.sprite = item.icon;
        scrollList = currentScrollList;

    }

    public void HandleButtonOnClick()
    {
        DebugUtil.debugGreen("BUTTON ITEM IN SOTRE IS CLICK");
    }
}
