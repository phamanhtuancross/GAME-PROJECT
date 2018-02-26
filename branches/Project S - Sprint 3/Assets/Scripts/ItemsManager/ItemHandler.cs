using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
{
    public ItemsType Type;
    public GameObject PanelText;
    public GameObject BuyButton;
    public Text NumText;

	void Update ()
    {
        int itemCount = UserData.GetItemCount(Type);

        if (itemCount > 0)
        {
            NumText.text = UserData.GetItemCount(Type).ToString();
            ActiveObject(PanelText);
        }
        else if(itemCount == 0)
        {
            ActiveObject(BuyButton);
        }
	}

    private void ActiveObject(GameObject active)
    {
        PanelText.SetActive(false);
        BuyButton.SetActive(false);

        active.SetActive(true);
    }
    
    public void OnItemClick()
    {
        if(UserData.GetItemCount(Type) > 0)
        {
            UserData.SubtractItem(Type, 1);
            ItemsManager.Instance.ResetButton(Type);
        }
    }
}
