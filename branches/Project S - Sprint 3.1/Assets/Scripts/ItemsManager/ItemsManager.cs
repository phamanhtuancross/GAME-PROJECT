using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemsType
{
    NONE_ITEM,
    BRUSH_ITEM,
    HAMMER_ITEM,
    CLOCK_ITEM
}
public class ItemsManager : Singleton<ItemsManager>
{

    public bool isBrushItemClick;
    public bool isHammerItemClick;
    public bool isClockItemClick;

    private int numberOfBrushItem;
    private int numberOfHammerItem;
    private int numberOfClockItem;

    public Text textNumberOfBrushItem;
    public Text textNumberOfHmamerItem;
    public Text textNumberOfClockItem;
  
    private void Awake()
    {
        isBrushItemClick  = false;
        isHammerItemClick = false;
        isClockItemClick  = false;

        //numberOfBrushItem = ItemAmount.MAXIMUM_OF_BRUSHER_ITEM;
        //numberOfHammerItem = ItemAmount.MAXIMUM_OF_HAMMER_ITEM;
        //numberOfClockItem = ItemAmount.MAXIMUM_OF_CLOCK_ITEM;

        numberOfBrushItem = UserData.GetItemCount(ItemsType.BRUSH_ITEM);
        numberOfHammerItem = UserData.GetItemCount(ItemsType.HAMMER_ITEM);
        numberOfClockItem = UserData.GetItemCount(ItemsType.CLOCK_ITEM);

        Chnaged_ChangeTextNumberOfItems(numberOfBrushItem, ItemsType.BRUSH_ITEM);
        Chnaged_ChangeTextNumberOfItems(numberOfHammerItem, ItemsType.HAMMER_ITEM);
        Chnaged_ChangeTextNumberOfItems(numberOfClockItem, ItemsType.CLOCK_ITEM);
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

                isBrushItemClick  = true;
                isHammerItemClick = false;
                isClockItemClick  = false;
                break;

            case ItemsType.HAMMER_ITEM:

                isBrushItemClick  = false;
                isHammerItemClick = true;
                isClockItemClick  = false;
                break;

            case ItemsType.CLOCK_ITEM:

                isBrushItemClick  = false;
                isHammerItemClick = false;
                isClockItemClick  = true;
                break;

            default:

                isBrushItemClick = false;
                isHammerItemClick = false;
                isClockItemClick = false;
                break;
        }
    }

    #region EVENTS ITEMS CLICK
    public void OnItemClick_BrushItemClick()
    {
        if (numberOfBrushItem > 0)
        {
            DebugUtil.debug("ON BRUSH ITEM CLICK");

            numberOfBrushItem--;
            Chnaged_ChangeTextNumberOfItems(numberOfBrushItem, ItemsType.BRUSH_ITEM);

            ResetButton(ItemsType.BRUSH_ITEM);
        }
    }

    public void OnItemClick_HammerItemClick()
    {
        if (numberOfHammerItem > 0)
        {
            DebugUtil.debugGreen("ON HAMMER ITEM CLICK");

            numberOfHammerItem--;
            Chnaged_ChangeTextNumberOfItems(numberOfHammerItem, ItemsType.HAMMER_ITEM);

            ResetButton(ItemsType.HAMMER_ITEM);
        }
    }

    public void OnItemClick_ClockItemClick()
    {
        if (numberOfClockItem > 0)
        {
            DebugUtil.debugYellow("ON CLOCK ITEM CLICK");

            numberOfClockItem--;
            Chnaged_ChangeTextNumberOfItems(numberOfClockItem, ItemsType.CLOCK_ITEM);

            ResetButton(ItemsType.CLOCK_ITEM);
        }
    }

    public ItemsType GetTypeOfItemClick()
    {
        if (isBrushItemClick)  return ItemsType.BRUSH_ITEM;
        if (isHammerItemClick) return ItemsType.HAMMER_ITEM;
        if (isClockItemClick)  return ItemsType.CLOCK_ITEM;

        return ItemsType.NONE_ITEM;
    }
    #endregion


}
