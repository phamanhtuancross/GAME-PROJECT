using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TargetSprite
{
    public TargetType type;
    public Sprite[] sprites;
}

public class MissionManager : Singleton<MissionManager>
{

    public static int BONUS_GOLD_BY_DIFFICULT = 200;
    public static int BONUS_GOLD_BY_LEVEL = 5;

    private int levelComplete;
    public List<BlockImpedient> lGrass;
    private bool isStep;
    private bool isTime;
    private bool isHaveGrass;

    public TargetSprite[] targets = new TargetSprite[Enum.GetNames(typeof(TargetType)).Length];
    //private string gameDataFileName = "data.json";

    Mission currentMission;
    public Mission CurrentMission
    {
        get
        {
            return currentMission;
        }
    }

    public bool IsStep
    {
        get
        {
            return isStep;
        }

        set
        {
            isStep = value;
        }
    }

    public bool IsTime
    {
        get
        {
            return isTime;
        }

        set
        {
            isTime = value;
        }
    }

    public bool IsHaveGrass
    {
        get
        {
            return isHaveGrass;
        }
    }

    public int LevelComplete
    {
        get
        {
            return levelComplete;
        }

        set
        {
            levelComplete = value;
        }
    }

    public void SetCurrentMission(int level)
    {
        try
        {
            lGrass = new List<BlockImpedient>();
            isHaveGrass = false;
            isStep = false;
            isTime = false;
            MissionController.Instance.LMissionLevel.TryGetValue(level, out currentMission);
            if (currentMission.Step > 0)
            {
                isStep = true;
                UIManager.Instance.StepText.enabled = true;
                UIManager.Instance.Step = currentMission.Step;
            }
            else if (currentMission.Time > 0)
            {
                UIManager.Instance.StepText.enabled = true;
                UIManager.Instance.Timer = currentMission.Time;
                isTime = true;
            }
            else
            {
                UIManager.Instance.StepText.enabled = false;
            }

            for (int i = 0; i < currentMission.LBlockImpedient.Count; i++)
            {
                int[] pos = currentMission.LBlockImpedient[i].PosInMap;
                if (currentMission.LBlockImpedient[i].BlockType == BlockType.BLOCK_GRASS)
                {
                    lGrass.Add(currentMission.LBlockImpedient[i]);
                    isHaveGrass = true;
                }
                UIManager.Instance.board.Grid[pos[0], pos[1]].GetComponent<Block>().ChangeType(currentMission.LBlockImpedient[i].BlockType);
            }
        }
        catch (ArgumentException e)
        {
            Debug.Log("Out Level");
        }
    }
    public int getGold()
    {
        int bonusByDifficult = BONUS_GOLD_BY_DIFFICULT * ((int)CurrentMission.Difficult + 1);
        int bonusByLevel = BONUS_GOLD_BY_LEVEL * CurrentMission.Level;
        int gold = bonusByDifficult + bonusByLevel;
        return gold;
    }
    private void Awake()
    {
        LevelComplete = UserData.getLevelComplete();
        Debug.Log(LevelComplete);
        IsStep = false;
        IsTime = false;
    }

    private void Update()
    {
        if (UIManager.Instance.Timer > 0 && isTime)
        {
            UIManager.Instance.Timer -= Time.deltaTime;
        }
        if (UIManager.Instance.Timer <= 0 && isTime)
        {
            UIManager.Instance.GameOver(true);
        }

        if (UIManager.Instance.Step == 0 && isStep)
        {
            if (!CheckLevelComplete())
            {
                UIManager.Instance.GameOver(true);
            }
        }
    }
    #region check_Mission
    // DestroyImpedient || BlockColor
    public void CheckMission(TargetType target, BlockType blocktype, int number)
    {
        for (int i = 0; i < CurrentMission.Targets.Count; i++)
        {
            if (CurrentMission.Targets[i].TargetType == target && CurrentMission.Targets[i].BlockType == blocktype)
            {
                StartCoroutine(UIManager.Instance.MissionIngame[i].UpdateDetail(number));
                break;
            }
        }
    }
    public void CheckMission(TargetType target, ShapeType shapeType)
    {
        for (int i = 0; i < CurrentMission.Targets.Count; i++)
        {
            if (CurrentMission.Targets[i].TargetType == target && CurrentMission.Targets[i].ShapeType == shapeType)
            {
                StartCoroutine(UIManager.Instance.MissionIngame[i].UpdateDetail(1));
                break;
            }
        }
    }
    public void CheckMission(TargetType target, int score)
    {
        for (int i = 0; i < CurrentMission.Targets.Count; i++)
        {
            if (CurrentMission.Targets[i].TargetType == target)
            {
                if (target == TargetType.Scored)
                {
                    StartCoroutine(UIManager.Instance.MissionIngame[i].UpdateDetail(score));
                }
                else
                {
                    StartCoroutine(UIManager.Instance.MissionIngame[i].UpdateDetail(1));
                }
                break;
            }
        }
    }
    public void CheckMission(TargetType target, Line type)
    {
        for (int i = 0; i < CurrentMission.Targets.Count; i++)
        {
            if (CurrentMission.Targets[i].TargetType == target && CurrentMission.Targets[i].LineType == type)
            {
                StartCoroutine(UIManager.Instance.MissionIngame[i].UpdateDetail(1));
                break;
            }
        }
    }
    #endregion
    public Sprite[] getTargetSprite(TargetType type)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (type == targets[i].type)
            {
                return targets[i].sprites;
            }
        }

        return null;
    }

    public bool CheckLevelComplete()
    {
        for (int i = 0; i < currentMission.Targets.Count; i++)
        {
            if (!UIManager.Instance.MissionIngame[i].IsComplete)
            {
                return false;
            }

        }
        UpdateMissionComplete();
        return true;
    }
    public void UpdateMissionComplete()
    {
        LevelComplete++;
        SetCurrentMission(LevelComplete + 1);
        UserData.updateLevelComplete();
        UIManager.Instance.LevelComplete(true);
    }
}
