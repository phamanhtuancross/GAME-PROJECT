using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class UIShopManagement : Singleton<UIShopManagement> {



    private int indexSlectionItem;
    private bool isExit;
    public GameObject selectionGameObject;
    public List<GameObject> listGameObject = new List<GameObject>();
    public Text price, playerCoins;
    public Button btnBuy;
    public GameObject panelSuccess;
    private int totalCoins;
    public GameObject panelFailed;
    public GameObject PriceMainObject;

    public bool IsExit
    {
        get
        {
            return isExit;
        }

        set
        {
            isExit = value;
        }
    }

    

    public int IndexSlectionItem
    {
        get
        {
            return indexSlectionItem;
        }

        set
        {
            indexSlectionItem = value;
        }
    }

    /// <summary>-------------------------------------------------------------------------------------------------------------------
    /// void RightSelection() : public
    /// The function for increaing the value of index selection item at current to 1 
    /// The fucntion implment when A RightClickButton on clicking
    /// </summary>
    public void RightSelection()
    {
        if (IndexSlectionItem == this.listGameObject.Count - 1)
        {
            IndexSlectionItem = 0;
        }
        else
        {
            IndexSlectionItem++;
        }
        selectionGameObject.GetComponent<Animator>().runtimeAnimatorController = listGameObject[IndexSlectionItem].GetComponent<Animator>().runtimeAnimatorController;
        ItemImforamtion temp = listGameObject[IndexSlectionItem].GetComponent<ItemImforamtion>();
        ChangeText(temp);
        Debug.Log(selectionGameObject.tag);
    }
    ///-----------------------------------------------------end of function--------------------------------------------------------

    /// <summary>------------------------------------------------------------------------------------------------------------------
    /// void RightSelection() : public
    /// The function for decreaing the value of index selection item at current to 1 
    /// The fucntion implment when A leftClickButton on clicking
    /// </summary>
    public void LeftSelection()
    {
        if (IndexSlectionItem == 0)
        {
            IndexSlectionItem = this.listGameObject.Count - 1;
        }
        else
        {
            IndexSlectionItem--;
        }
        selectionGameObject.GetComponent<Animator>().runtimeAnimatorController = listGameObject[IndexSlectionItem].GetComponent<Animator>().runtimeAnimatorController;
        ItemImforamtion temp = listGameObject[IndexSlectionItem].GetComponent<ItemImforamtion>();
        ChangeText(temp);


        Debug.Log(selectionGameObject.tag);
    }
    //------------------------------------------------------end of function-----------------------------------------------------------
    // Use this for initialization
    void Start() {
        this.PriceMainObject.SetActive(false);
        LoadData();
        ItemsManager.Instance.SetItemsData(GetListItemImformation());
        this.panelSuccess.SetActive(false);
        this.panelFailed.SetActive(false);
        this.totalCoins = PlayerPrefs.GetInt("totalCoins");
        Debug.Log("total coins " + totalCoins);

        selectionGameObject.GetComponent<Animator>().runtimeAnimatorController = listGameObject[0].GetComponent<Animator>().runtimeAnimatorController;
        playerCoins.text =  totalCoins+ "";
        selectionGameObject.GetComponent<ItemImforamtion>().price = listGameObject[0].GetComponent<ItemImforamtion>().price;
        selectionGameObject.GetComponent<ItemImforamtion>().isBought = listGameObject[0].GetComponent<ItemImforamtion>().isBought;
        this.ChangeText(selectionGameObject.GetComponent<ItemImforamtion>());
        //Debug.Log("total coinst : " + BirdController.Instance.TotalCoins);
        // Debug.Log(BirdController.i)
    }
    // Update is called once per frame
    void Update() {
    }

    /// <summary>--------------------------------------------------------------------------------------------------------------------
    ///  void ChangeText(ItemImforamtion temp) : public
    ///  The function for change the value of text when the selected items change
    ///  if the cureent item which our owned then reset the text of buy button and set unactive for this button
    /// </summary>
    /// <param name="temp"></param>
    public void ChangeText(ItemImforamtion temp)
    {
        if (temp.isBought)
        {
            this.btnBuy.GetComponentInChildren<Text>().text = "USE";
            this.PriceMainObject.SetActive(false);
        }
        else
        {
            this.btnBuy.GetComponentInChildren<Text>().text = "BUY ITEM";
            this.PriceMainObject.SetActive(true);
        }
        this.price.text = temp.price + "";

    }
    //--------------------------------------------------------------end of function--------------------------------------------------

    List<ItemImforamtion> GetListItemImformation()
    {
        List<ItemImforamtion> result = new List<ItemImforamtion>();
        foreach (GameObject item in listGameObject)
        {
            ItemImforamtion temp = item.GetComponent<ItemImforamtion>();
            result.Add(temp);
        }
        return result;
    }


    public void ChangetStateBuy(bool isBought)
    {
        this.listGameObject[IndexSlectionItem].GetComponent<ItemImforamtion>().isBought = isBought ;
    }
    /// <summary>--------------------------------------------------------------------------------------------------------------------
    /// void ButtonBuyOnClick() : show a panel when the buy button click
    /// </summary>
    public void ButtonBuyOnClick()
    {
        ItemImforamtion temp = listGameObject[IndexSlectionItem].GetComponent<ItemImforamtion>();
        if (temp != null)
        {

            if (temp.isBought == false)
            {
                if (totalCoins >= temp.price)
                {
                    totalCoins -= temp.price;
                    this.playerCoins.text = totalCoins + "";
                    PlayerPrefs.SetInt("totalCoins", totalCoins);
                    ChangetStateBuy(true);
                    temp.isBought = true;
                    ChangeText(temp);
                    this.panelSuccess.SetActive(true);
                    ItemsManager.Instance.SetItemsData(GetListItemImformation());
                }
                else
                {
                    this.panelFailed.SetActive(true);
                }
            }
            else
            {
                BirdController.Instance.Anim.runtimeAnimatorController = selectionGameObject.GetComponent<Animator>().runtimeAnimatorController;
            }
            
        }
    }
    //----------------------------------------------------------------end of function-------------------------------------------------

   

    public void ButtonOKOnClick()
    {
        this.panelSuccess.SetActive(false);
        this.panelFailed.SetActive(false);
    }
    //----------------------------------------------------------------end of fucntion-------------------------------------------------

    /// <summary>----------------------------------------------------------------------------------------------------------------------
    /// void ButtonExitOnClick() : public
    /// The fucntion:
    /// isExit = true when  button Exit clicked
    /// </summary>
    public void ButtonExitOnClick()
    {
        // this.isExit = true;
        //Application.Quit();
        SceneManager.UnloadSceneAsync("ShopItems");
        Camera.main.GetComponent<AudioListener>().enabled = true;

        UIManager.Instance.UpdateCoinText();

    }
    //-----------------------------------------------------------------enn of fucntion-------------------------------------------------

    public void LoadData()
    {
        ItemsManager.Instance.LoadListItems();
        ItemsData listItems = ItemsManager.Instance.listItems;  
    }

}


