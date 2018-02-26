using UnityEngine;
using System.Collections;

public class FriendsPanel : MonoBehaviour
{
    public void ForceDropDown()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ANSPopUp"))
            GetComponent<Animator>().SetTrigger("Run");
    }
}
