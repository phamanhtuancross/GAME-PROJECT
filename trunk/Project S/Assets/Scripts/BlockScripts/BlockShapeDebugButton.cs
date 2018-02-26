using UnityEngine;
using System.Collections;

public class BlockShapeDebugButton : MonoBehaviour
{
    public BlockShape shape;

    public ShapeType optionalShapeType;
    public Direction optionalDirection;
    public BlockType optionalBlockType;

    public void ClearShape()
    {
        shape.EatShape();
    }

    public void InitShapeRamdom()
    {
        ShapeType shapeType = BlockFactory.Instance.GetRandomEnum<ShapeType>();
        Direction direction = BlockFactory.Instance.GetRandomEnum<Direction>();
        BlockType blockType = BlockFactory.Instance.GetRandomEnum<BlockType>();
        //DebugUtil.debug("Random shape: " + shapeType + " - " + direction + " - " + blockType);

        shape.InitShape(shapeType, direction, blockType);
    }

    public void InitShapeWithOptional()
    {
        shape.InitShape(optionalShapeType, optionalDirection, optionalBlockType);
        //DebugUtil.debug("Option shape: " + optionalShapeType + " - " + optionalDirection + " - " + optionalBlockType);
    }

    private void Start()
    {
        InitShapeRamdom();
    }
}
