using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInforDataManager : MonoBehaviour
{


    public GameObject mission_No1;
    public GameObject mission_No2;
    public GameObject mission_No3;
    public GameObject panelShowHint;
    public Text textMission;

    public void Update()
    {
        bool isMission_No1Holding = mission_No1.GetComponent<MissionInforData>().isHolding;
        bool isMission_No2Holding = mission_No2.GetComponent<MissionInforData>().isHolding;
        bool isMission_No3Holding = mission_No3.GetComponent<MissionInforData>().isHolding;

        if (isMission_No1Holding)
        {

            DebugUtil.debugGreen("MISSION 1 HOLDING");
            panelShowHint.SetActive(true);
            textMission.text = SetUpItemsImformation(0);
        }
        else if (isMission_No2Holding)
        {
            DebugUtil.debugGreen("MISSION 2 HODLING");
            panelShowHint.SetActive(true);
            textMission.text = SetUpItemsImformation(1);
        }
        else if (isMission_No3Holding)
        {
            DebugUtil.debugGreen("MISSION 3 HODLING");
            panelShowHint.SetActive(true);
            textMission.text = SetUpItemsImformation(2);
        }

        else
        {
            panelShowHint.SetActive(false);
        }


    }
    public string SetUpItemsImformation(int indexOfMission)
    {

        bool isCancle = false;
        String resultMissiionDatas = "";
        Mission currentMission = MissionManager.Instance.CurrentMission;
        List<Target> listOfTarget = currentMission.targets;
        if (listOfTarget.Count >= indexOfMission)
        {
            Target currentTarget = listOfTarget[indexOfMission];

            String blockTypeString = "";
            switch (currentTarget.blockType)
            {
                case BlockType.BLOCK_BLUE:
                    blockTypeString = "BLOCK BLUE";
                    break;

                case BlockType.BLOCK_BOARD:
                    blockTypeString = "BLOCK BOARD";
                    break;

                case BlockType.BLOCK_GRASS:
                    blockTypeString = "BLOCK GRASS";

                    break;

                case BlockType.BLOCK_GREEN:
                    blockTypeString = "BLOCK GREEN";
                    break;

                case BlockType.BLOCK_ORANGE:
                    blockTypeString = "BLOCK ORANGE";
                    break;

                case BlockType.BLOCK_PURPLE:
                    blockTypeString = "BLOCK PURPLE";
                    break;

                case BlockType.BLOCK_RED:
                    blockTypeString = "BLOCK RED";
                    break;

                case BlockType.BLOCK_ROCK:
                    blockTypeString = "BLOCK ROCK";
                    break;

                case BlockType.BLOCK_YELLOW:
                    blockTypeString = "BLOCK YELLOW";
                    break;

                case BlockType.EMPTY:
                    blockTypeString = "BLOCK EMPTY";
                    break;

                default:
                    break;
            }

            switch (currentTarget.targetType)
            {
                case TargetType.DestroyImpedient:
                    {
                        resultMissiionDatas = "DESTROY  " + currentTarget.amount + " " + blockTypeString + "";
                    }
                    break;

                case TargetType.EatBlockColor:
                    resultMissiionDatas = "DESTROY " + currentTarget.amount + " " + blockTypeString + "";
                    break;

                case TargetType.EatBlockLine:
                    {
                        switch (currentTarget.lineType)
                        {
                            case Line.Collum:
                                resultMissiionDatas = "DESTROY " + currentTarget.amount + " COLUMNS";
                                break;
                            case Line.Row:
                                resultMissiionDatas = "DESTROY " + currentTarget.amount + " ROWS";
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case TargetType.EatCombo:
                    {
                        resultMissiionDatas = "DESTROY " + currentTarget.amount + " ROWS OR COLUMS NEAREST";
                    }
                    break;

                case TargetType.PutBlockShape:
                    {
                        resultMissiionDatas = "PUT " + currentTarget.amount + " SHAPES TYPE INTO BOARD"; 
                    }
                    break;

                case TargetType.Scored:
                    {
                        resultMissiionDatas = "GET " + currentTarget.amount + " POINTS";
                    }
                    break;

                default:
                    break;
            }
        }
        DebugUtil.debugYellow(resultMissiionDatas);
        return resultMissiionDatas;
    }
}
