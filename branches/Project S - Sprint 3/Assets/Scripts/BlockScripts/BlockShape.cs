using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShapeType
{
    SQUARE1,
    SQUARE2,
    SQUARE3,
    L2,
    L3,
    LINE2,
    LINE3,
    LINE4
}

public enum Direction
{
    TOP_LEFT,
    TOP_RIGHT,
    BOT_LEFT,
    BOT_RIGHT
}

[System.Serializable]
public class BlockShape : MonoBehaviour
{
    public GameObject[,] blocks;
    public GameObject blockEmptyPrefab;
    public Transform container;

    //shape fields
    private ShapeType shape;
    private BlockType block;
    private Direction direction;
    private ShapeHolderIndex indexShpaeHolder;

    private bool canPlace;
    private bool placeSuccess;

    //readonly encaps
    public ShapeType Shape
    {
        get
        {
            return shape;
        }
    }
    public BlockType Block
    {
        get
        {
            return block;
        }
    }
    public Direction Direction
    {
        get
        {
            return direction;
        }
    }

    public ShapeHolderIndex IndexShpaeHolder
    {
        get
        {
            return indexShpaeHolder;
        }

        set
        {
            indexShpaeHolder = value;
        }
    }
    /// <summary>
    /// Class use for ShapeDictionary as TKey
    /// </summary>
    public partial class ShapeDirectionPair
    {
        public ShapeType shape;
        public Direction direction;

        public ShapeDirectionPair(ShapeType shapeType, Direction direction)
        {
            this.shape = shapeType;
            this.direction = direction;
        }

        public bool Equals(ShapeDirectionPair obj)
        {
            return obj != null && shape == obj.shape && direction == obj.direction;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ShapeDirectionPair);
        }

        public override int GetHashCode()
        {
            //using "Cantor pairing function" to generate hashcode
            float result = 0.5f * ((float)shape * (float)direction) * ((float)shape + (float)direction + 1) + (float)shape;
            return (int)result;
        }
    }

    private Dictionary<ShapeDirectionPair, Vector2Int[]> shapeDictionary;

    private static List<ShapeDirectionPair> lBlockShape;
    public static List<ShapeDirectionPair> LBlockShape
    {
        get
        {
            return lBlockShape;
        }
    }

    private void InitListBlockShape()
    {
        lBlockShape = new List<ShapeDirectionPair>();
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.SQUARE1, Direction.BOT_LEFT)); //0
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE2, Direction.TOP_LEFT)); // 1
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE2, Direction.BOT_LEFT)); // 2
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.SQUARE2, Direction.BOT_LEFT)); // 3
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE3, Direction.TOP_LEFT)); // 4
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE3, Direction.BOT_LEFT)); // 5
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE4, Direction.TOP_LEFT)); // 6
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.LINE4, Direction.BOT_LEFT)); // 7
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L2, Direction.TOP_LEFT)); //8
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L2, Direction.TOP_RIGHT)); //9
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L2, Direction.BOT_LEFT)); //10
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L2, Direction.BOT_RIGHT)); //11
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.SQUARE3, Direction.BOT_LEFT)); //12
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L3, Direction.TOP_LEFT)); //13
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L3, Direction.TOP_RIGHT)); //14
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L3, Direction.BOT_LEFT)); //15
        lBlockShape.Add(new ShapeDirectionPair(ShapeType.L3, Direction.BOT_RIGHT)); //16
    }

    private void Awake()
    {
        InitShapeDictionary();
        InitListBlockShape();

        placeSuccess = false;

        blocks = new GameObject[4, 4];

        //clear childs
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        //default
        shape = ShapeType.SQUARE1;
        block = BlockType.EMPTY;
        direction = Direction.BOT_LEFT;


        for (int row = 0; row < 4; row++)
        {
            for (int cell = 0; cell < 4; cell++)
            {
                GameObject block = GameObject.Instantiate(blockEmptyPrefab, container);
                blocks[row, cell] = block;
            }
        }
    }

    private void Start()
    {
        grid = GetComponentInChildren<GridLayoutGroup>();
    }



    public void InitShapeDictionary()
    {
        if (shapeDictionary != null)    //initialized before
            return;

        shapeDictionary = new Dictionary<BlockShape.ShapeDirectionPair, UnityEngine.Vector2Int[]>();

        //SQUARE1
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE1, Direction.BOT_LEFT),
                new Vector2Int[] { new Vector2Int(0, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE1, Direction.BOT_RIGHT),
                new Vector2Int[] { new Vector2Int(0, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE1, Direction.TOP_LEFT),
                new Vector2Int[] { new Vector2Int(0, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE1, Direction.TOP_RIGHT),
                new Vector2Int[] { new Vector2Int(0, 0) });
        }
        //SQUARE2
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE2, Direction.BOT_LEFT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE2, Direction.BOT_RIGHT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE2, Direction.TOP_RIGHT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE2, Direction.TOP_LEFT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)});
        }
        //SQUARE3
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE3, Direction.BOT_LEFT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 2),
                        new Vector2Int(2, 0),
                        new Vector2Int(2, 1),
                        new Vector2Int(2, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE3, Direction.BOT_RIGHT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 2),
                        new Vector2Int(2, 0),
                        new Vector2Int(2, 1),
                        new Vector2Int(2, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE3, Direction.TOP_LEFT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 2),
                        new Vector2Int(2, 0),
                        new Vector2Int(2, 1),
                        new Vector2Int(2, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.SQUARE3, Direction.TOP_RIGHT),
                new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                        new Vector2Int(1, 2),
                        new Vector2Int(2, 0),
                        new Vector2Int(2, 1),
                        new Vector2Int(2, 2)});
        }

        //L2
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L2, Direction.BOT_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L2, Direction.BOT_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 1),
                                new Vector2Int(1, 1),
                                new Vector2Int(0, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L2, Direction.TOP_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(1, 1) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L2, Direction.TOP_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 1),
                                new Vector2Int(1, 1),
                                new Vector2Int(1, 0) });
        }
        //L3
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L3, Direction.BOT_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L3, Direction.BOT_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2),
                                new Vector2Int(1, 2),
                                new Vector2Int(2, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L3, Direction.TOP_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0),
                                new Vector2Int(2, 1),
                                new Vector2Int(2, 2)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.L3, Direction.TOP_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(2, 0),
                                new Vector2Int(2, 1),
                                new Vector2Int(2, 2),
                                new Vector2Int(0, 2),
                                new Vector2Int(1, 2)});
        }

        //LINE2
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE2, Direction.BOT_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE2, Direction.BOT_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE2, Direction.TOP_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE2, Direction.TOP_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0)});
        }
        //LINE3
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE3, Direction.BOT_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE3, Direction.BOT_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE3, Direction.TOP_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0) });
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE3, Direction.TOP_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0) });
        }
        //LINE4
        {
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE4, Direction.BOT_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2),
                                new Vector2Int(0, 3)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE4, Direction.BOT_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(0, 1),
                                new Vector2Int(0, 2),
                                new Vector2Int(0, 3)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE4, Direction.TOP_LEFT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0),
                                new Vector2Int(3, 0)});
            shapeDictionary.Add(
                new ShapeDirectionPair(ShapeType.LINE4, Direction.TOP_RIGHT),
                new Vector2Int[] {
                                new Vector2Int(0, 0),
                                new Vector2Int(1, 0),
                                new Vector2Int(2, 0),
                                new Vector2Int(3, 0)});
        }

        DebugUtil.debugGreen("Init Shape Dictionary Successfully!");
    }

    public void InitShape(ShapeType shape, Direction direction, BlockType block)
    {
        ClearShape();

        this.shape = shape;
        this.block = block;
        this.direction = direction;
        placeSuccess = false;

        ReplaceBlock(shapeDictionary[new ShapeDirectionPair(shape, direction)], block);
    }

    public void ClearShape()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                blocks[x, y].GetComponent<Block>().ChangeType(BlockType.EMPTY);
            }
        }
    }

    public void EatShape()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (!blocks[x, y].GetComponent<Block>().IsClearing)
                    blocks[x, y].GetComponent<Block>().ClearBlock();
            }
        }
    }

    public void ReplaceBlock(Vector2Int[] indexes, BlockType type)
    {
        foreach (Vector2Int index in indexes)
        {
            blocks[index.x, index.y].GetComponent<Block>().ChangeType(type);
        }
    }

    public LayerMask CastLayer;
    private GridLayoutGroup grid;
    
    public void SelectBlock()
    {
        Debug.Log("StartDrag");
        ResetPos();
        GetComponent<Animator>().SetTrigger("StartDrag");
    }

    public void UnSelectBlock()
    {
        Debug.Log("DragFail");
        if(placeSuccess)
        {
            GetComponent<Animator>().SetTrigger("PlaceSuccess");
        }
        else
            GetComponent<Animator>().SetTrigger("PlaceFail");
    }

    public void MoveBlock(Vector3 target)
    {
        Vector3 offset = new Vector3(0, 1.5f, 10);
        target += offset;
        transform.position = Vector3.MoveTowards(transform.position, target, 15f * Time.fixedDeltaTime);
    }

    public void PlaceBlock()
    {
        RaycastHit2D hit;
        Vector2Int temp = new Vector2Int(-1, -1);
        Block placePos = null;
        hit = Physics2D.Raycast(blocks[0, 0].transform.position, Vector2.zero, 100f, CastLayer);
        if (hit.collider != null)
        {
            placePos = hit.collider.GetComponent<Block>();
            temp = placePos.index;
            canPlace = (BoardGameManager.Instance.CheckValidPosition_ShapePosition(temp.x, temp.y, gameObject));
        }

        if (canPlace)
        {
            GetComponent<Collider2D>().enabled = false;
            BoardGameManager.Instance.Add_ShapeToBoard(temp.x, temp.y, gameObject);
            ResetShape();
            BoardGameManager.Instance.Implementation_BoardAction();
            canPlace = false;
            placeSuccess = true;
        }

        UnSelectBlock();
    }

    public void ResetPos()
    {
        StopCoroutine(MovePos(Vector3.zero, 1.0f / 6.0f));
        transform.localPosition = Vector3.zero;
    }

    public void LerpResetPos()
    {
        StartCoroutine(MovePos(Vector3.zero, 1.0f / 6.0f));
    }

    public IEnumerator MovePos(Vector3 target, float time)
    {
        while(transform.localPosition != target)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime / time);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void ResetShape()
    {
        InitShape(this.shape, this.direction, BlockType.EMPTY);
    }
}
