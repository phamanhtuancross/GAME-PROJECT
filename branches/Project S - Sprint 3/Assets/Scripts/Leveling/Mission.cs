using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BlockImpedient
{
    public BlockType blockType;
    public int[] posInMap;

    public BlockImpedient(BlockType blockType, int[] pos)
    {
        this.blockType = blockType;
        this.posInMap = pos;
    }

    public BlockType BlockType
    {
        get
        {
            return blockType;
        }
    }

    public int[] PosInMap
    {
        get
        {
            return posInMap;
        }
    }
}
public enum Item
{
    Hammer,
    Swob,
    Clock
}
public enum Difficult
{
    Easy = 1, Normal = 2, Hard = 3, Extremely = 4
}

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class Mission : ScriptableObject
{

    public int level;
    public List<Target> targets;
    public List<BlockImpedient> lBlockImpedient;
    private int highScored;
    public Difficult difficult;
    public int step;
    public float time;
    #region Get Data


    public List<Target> Targets
    {
        get
        {
            return targets;
        }
    }

    public List<BlockImpedient> LBlockImpedient
    {
        get
        {
            return lBlockImpedient;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int HighScored
    {
        set
        {
            highScored = value;
        }

        get
        {
            return highScored;
        }
    }

    public Difficult Difficult
    {
        get
        {
            return difficult;
        }
    }

    public int Step
    {
        get
        {
            return step;
        }
    }

    public float Time
    {
        get
        {
            return time;
        }
    }
    #endregion
    public Mission(int level, List<Target> targets, List<BlockImpedient> lBlockImpedient, int highScored, Difficult difficult, int step, float time)
    {
        this.level = level;
        this.targets = targets;
        this.lBlockImpedient = lBlockImpedient;
        this.highScored = highScored;
        this.difficult = difficult;
        this.step = step;
        this.time = time;
    }
}
