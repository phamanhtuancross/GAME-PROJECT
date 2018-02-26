using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum ItemsType
{
    NONE_ITEM = 0,
    BRUSH_ITEM = 1,
    HAMMER_ITEM = 2,
    CLOCK_ITEM = 3
}
public class ItemsManager : Singleton<ItemsManager>
{

    public bool isBrushItemClick;
    public bool isHammerItemClick;
    public bool isClockItemClick;

    public int numberOfBrushItem;
    public int numberOfHammerItem;
    public int numberOfClockItem;

    public Text textNumberOfBrushItem;
    public Text textNumberOfHmamerItem;
    public Text textNumberOfClockItem;

    public Image spriteNumberOfBrushItem;
    public Image spriteNumberOfHammerItem;
    public Image spriteNumberOfClockItem;

    public Sprite spriteByItem;

    public GameObject mainCanvas;

    private void Awake()
    {
        isBrushItemClick = false;
        isHammerItemClick = false;
        isClockItemClick = false;

        //numberOfBrushItem = ItemAmount.MAXIMUM_OF_BRUSHER_ITEM;
        //numberOfHammerItem = ItemAmount.MAXIMUM_OF_HAMMER_ITEM;
        //numberOfClockItem = ItemAmount.MAXIMUM_OF_CLOCK_ITEM;

        //numberOfBrushItem = UserData.GetItemCount(ItemsType.BRUSH_ITEM);
        //numberOfHammerItem = UserData.GetItemCount(ItemsType.HAMMER_ITEM);
        //numberOfClockItem = UserData.GetItemCount(ItemsType.CLOCK_ITEM);

        //Chnaged_ChangeTextNumberOfItems(numberOfBrushItem, ItemsType.BRUSH_ITEM);
        //Chnaged_ChangeTextNumberOfItems(numberOfHammerItem, ItemsType.HAMMER_ITEM);
        //Chnaged_ChangeTextNumberOfItems(numberOfClockItem, ItemsType.CLOCK_ITEM);
    }

    public void Chnaged_ChangeTextNumberOfItems(int number, ItemsType typeOfItem)
    {
        switch (typeOfItem)
        {
            case ItemsType.BRUSH_ITEM:
                textNumberOfBrushItem.text = number + "";
                break;

            case ItemsType.HAMMER_ITEM:
                textNumberOfHmamerItem.text = number + "";
                break;

            case ItemsType.CLOCK_ITEM:
                textNumberOfClockItem.text = number + "";
                break;

            default:
                break;
        }
    }

    public void ResetButton(ItemsType typeOfItem)
    {
        switch (typeOfItem)
        {
            case ItemsType.BRUSH_ITEM:

                isBrushItemClick = true;
                isHammerItemClick = false;
                isClockItemClick = false;
                break;

            case ItemsType.HAMMER_ITEM:

                isBrushItemClick = false;
                isHammerItemClick = true;
                isClockItemClick = false;
                break;

            case ItemsType.CLOCK_ITEM:

                isBrushItemClick = false;
                isHammerItemClick = false;
                isClockItemClick = true;
                break;

            default:

                isBrushItemClick = false;
                isHammerItemClick = false;
                isClockItemClick = false;
                break;
        }
    }

    public void Update()
    {
        //if (numberOfBrushItem == 0)
        //{
        //    this.spriteNumberOfBrushItem.sprite = spriteByItem;
        //}

        //if (numberOfHammerItem == 0)
        //{
        //    this.spriteNumberOfHammerItem.sprite = spriteByItem;
        //}

        //if (numberOfClockItem == 0)
        //{
        //    this.spriteNumberOfClockItem.sprite = spriteByItem;
        //}

    }
    #region EVENTS ITEMS CLICK
    public void OnItemClick_BrushItemClick()
    {
        if (UIManager.Instance.LockPlay)
            return;

        if (UserData.GetItemCount(ItemsType.BRUSH_ITEM) > 0)
        {
            mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("Show");
            DebugUtil.debug("ON BRUSH ITEM CLICK");
            Chnaged_ChangeTextNumberOfItems(numberOfBrushItem, ItemsType.BRUSH_ITEM);

            UsePanelController.Instance.ChangedItemSpriteByTypeItems(ItemsType.BRUSH_ITEM);

            ResetButton(ItemsType.BRUSH_ITEM);
        }
        else
        {
            //Show store panel
        }

    }

    public void OnItemClick_HammerItemClick()
    {
        if (UIManager.Instance.LockPlay)
            return;

        if (UserData.GetItemCount(ItemsType.HAMMER_ITEM) > 0)
        {
            mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("Show");
            DebugUtil.debugGreen("ON HAMMER ITEM CLICK");
            Chnaged_ChangeTextNumberOfItems(numberOfHammerItem, ItemsType.HAMMER_ITEM);
            UsePanelController.Instance.ChangedItemSpriteByTypeItems(ItemsType.HAMMER_ITEM);

            ResetButton(ItemsType.HAMMER_ITEM);

        }
        else
        {
            //mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("ShowStore");
        }
    }

    public void OnItemClick_ClockItemClick()
    {
        if (UIManager.Instance.LockPlay)
            return;

        if (UserData.GetItemCount(ItemsType.CLOCK_ITEM) > 0)
        {
            mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("Show");
            DebugUtil.debugYellow("ON CLOCK ITEM CLICK");

            Chnaged_ChangeTextNumberOfItems(numberOfClockItem, ItemsType.CLOCK_ITEM);

            UsePanelController.Instance.ChangedItemSpriteByTypeItems(ItemsType.CLOCK_ITEM);

            ResetButton(ItemsType.CLOCK_ITEM);
        }
        else
        {
            //show store panel
        }
    }

    public ItemsType GetTypeOfItemClick()
    {
        if (isBrushItemClick) return ItemsType.BRUSH_ITEM;
        if (isHammerItemClick) return ItemsType.HAMMER_ITEM;
        if (isClockItemClick) return ItemsType.CLOCK_ITEM;

        return ItemsType.NONE_ITEM;
    }
    #endregion  

}
