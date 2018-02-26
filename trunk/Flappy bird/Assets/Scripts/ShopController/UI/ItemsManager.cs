using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ItemsManager : Singleton<ItemsManager>
{

    public ItemsData listItems;


    public void LoadListItems()
    {
        listItems = JsonUtility.FromJson<ItemsData>(PlayerPrefs.GetString("listItems"));
    }
    public void SetItemsData(List<ItemImforamtion> listItemImformation)
    {
        if (listItems == null)
        {
            listItems = new ItemsData();
        }
        this.listItems.items = listItemImformation.ToArray();
        SaveListItems();
    }

    public void SaveListItems()
    {
        string result = JsonUtility.ToJson(listItems);
        PlayerPrefs.SetString("listItems", result);
    }
}