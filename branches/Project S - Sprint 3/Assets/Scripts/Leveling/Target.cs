using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TargetType
{
    DestroyImpedient, // BlockType + Amount
    EatBlockColor,  // BlockType + Amount
    EatBlockLine, //Line + Amount
    Scored, // Amount
    PutBlockShape, // BlockShape + Amount
    EatCombo, // int + int
}

[System.Serializable]
public enum Line
{
    Collum, Row
}

[System.Serializable]
public class Target
{
    public TargetType targetType;
    public int amount; // may be score
    public BlockType blockType;
    public Line lineType;
    public ShapeType shapeType;
    public int coomboX;
    public TargetType TargetType
    {
        get
        {
            return targetType;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public BlockType BlockType
    {
        get
        {
            return blockType;
        }
    }

    public Line LineType
    {
        get
        {
            return lineType;
        }
    }

    public ShapeType ShapeType
    {
        get
        {
            return shapeType;
        }
    }

    public int ComboX
    {
        get
        {
            return coomboX;
        }
    }

    // init Scored
    public Target(TargetType targetType, int amount)
    {
        this.targetType = targetType;
        this.amount = amount;
    }

    //init EatBlockColor + DestroyImpedient
    public Target(TargetType targetType, BlockType blockType, int amount)
    {
        this.targetType = targetType;
        this.amount = amount;
        this.blockType = blockType;
    }

    //init EatBlockLine
    public Target(TargetType targetType, Line lineType, int amount)
    {
        this.targetType = targetType;
        this.amount = amount;
        this.lineType = lineType;
    }

    //init PutBlockShape
    public Target(TargetType targetType, ShapeType shapeType, int amount)
    {
        this.targetType = targetType;
        this.amount = amount;
        this.shapeType = shapeType;
    }
    //
    public Target(TargetType targetType, int combo, int amount)
    {
        this.targetType = targetType;
        this.amount = amount;
        this.coomboX = combo;
    }
}

