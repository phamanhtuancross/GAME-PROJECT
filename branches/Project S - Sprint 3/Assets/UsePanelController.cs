using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsePanelController : Singleton<UsePanelController>
{

    // Use this for initialization

    public Sprite brushSprite;
    public Sprite hammerSprite;
    public Sprite clockSprite;

    public Image itemSprite;


    public bool isUseButtonClick = false;

    public void OnUseButtonClick()
    {
        ItemsManager.Instance.mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("Hide");
        isUseButtonClick = true;
        
        
    }

    public void OnsCancleButtonClick()
    {
        ItemsManager.Instance.mainCanvas.gameObject.GetComponent<Animator>().SetTrigger("Hide");
    }


    public void ChangedItemSpriteByTypeItems(ItemsType typeOfItem)
    {
        switch (typeOfItem)
        {
            case ItemsType.BRUSH_ITEM:
                itemSprite.sprite = brushSprite;
                break;

            case ItemsType.HAMMER_ITEM:
                itemSprite.sprite = hammerSprite;
                break;

            case ItemsType.CLOCK_ITEM:
                itemSprite.sprite = clockSprite;
                break;

            default:
                break;
        }
    }
}
