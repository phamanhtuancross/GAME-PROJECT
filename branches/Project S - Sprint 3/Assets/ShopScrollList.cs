using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ItemInStoreInfor
{
    public ItemsType typeOfItem;
    public string nameOfItem;
    public float price;
    public Sprite icon;
}
public class ShopScrollList : MonoBehaviour {

    public  List<ItemInStoreInfor> listOfItem;
    public Transform contentPanel;
    public SimpleObjectPool buttonObjectPool;

    private void Start()
    {
        RefreshDisplay();   
    }

    private void RefreshDisplay()
    {
        RemoveButton();
        AddButtons();
    }

    private void RemoveButton()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    public void AddButtons()
    {
        for (int index = 0; index < listOfItem.Count; index++)
        {
            ItemInStoreInfor item = listOfItem[index];
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel);
            SampleButton sampleButton = newButton.GetComponent<SampleButton>();
            sampleButton.SetUpForSampleButton(item, this);

        }
    }


    
    
}
