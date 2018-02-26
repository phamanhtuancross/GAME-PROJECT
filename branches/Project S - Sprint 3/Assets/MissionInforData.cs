using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionInforData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        DebugUtil.debugGreen("IS HOLDING OBJECT MISSION INFOR");
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DebugUtil.debugYellow("IS RELEASING OBJECT MISSION INFOR");
        isHolding = false;
    }

}
