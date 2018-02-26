using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;


public class BlockStateOnBoard
{
    public Vector2Int index;
    public BlockType type;

    public BlockStateOnBoard(Vector2Int _index, BlockType _type)
    {
        this.index = _index;
        this.type = _type;
    }
}

public class HistoryStateOnBoard
{
    public ActionOnBoard action;
    public List<BlockStateOnBoard> blocks;

    public HistoryStateOnBoard(ActionOnBoard _action, List<BlockStateOnBoard> _blocks)
    {
        this.action = _action;
        this.blocks = _blocks;
    }

    public HistoryStateOnBoard(ActionOnBoard _action, Vector2Int startPosition, GameObject shape)
    {
        this.blocks = new List<BlockStateOnBoard>();
        this.action = _action;

        BlockShape blockShape = shape.GetComponent<BlockShape>();
        if (blockShape)
        {
            for (int i = 0; i < Define.SIZE_OF_SHAPE; i++)
            {
                for (int j = 0; j < Define.SIZE_OF_SHAPE; j++)
                {
                    BlockType typeOfBlock = blockShape.blocks[i, j].GetComponent<Block>().type;
                    if (typeOfBlock != BlockType.EMPTY)
                    {
                        int rowIndex = startPosition.x + i;
                        int colIndex = startPosition.y + j;

                        this.blocks.Add(new BlockStateOnBoard(new Vector2Int(rowIndex, colIndex), typeOfBlock));
                    }
                }
            }
        }
    }

}


public class BoardGameManager : Singleton<BoardGameManager>
{
    public SFXSounds sfxSounds;

    public static int gridWidth = 10;
    public static int gridHeight = 10;
    private float cellSize;
    public bool isGameOver;
    private bool isCanSwipe;

    public Transform container;
    private GameObject[,] grid;
    private BlockType[,] olderGrid;
    public GameObject block;

    #region VARIBALE DEBUG
    private int count;
    #endregion

    private List<GameObject> clearBlock = new List<GameObject>();
    private List<GameObject> addedBlock = new List<GameObject>();

    private List<HistoryStateOnBoard> listOfPreviousStepOnBoard = new List<HistoryStateOnBoard>();

    //public BlockShape BlockShape_4x4_1;
    //public BlockShape BlockShape_4x4_2;
    //public BlockShape BlockShape_4x4_3;

    Vector2Int index;
    public void Update()
    {

        ItemsType typeOfItemClicked = ItemsManager.Instance.GetTypeOfItemClick();
        switch (typeOfItemClicked)
        {
            case ItemsType.BRUSH_ITEM:
                {

                    Vector2 firstPostion = new Vector2();
                    Vector2 currentPosition = new Vector2();

                    //DebugUtil.debugYellow("IS CAN SWIPE :" + isCanSwipe);

                    if (Input.GetMouseButtonDown(0))
                    {

                        //Change_TypeOfOlderGrid();
                        firstPostion = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                        // DebugUtil.debugYellow(firstPostion.x + " | " + firstPostion.y);
                        index = GetIndex_GetIndexFromMousePosition();
                        isCanSwipe = true;

                    }

                    if (isCanSwipe == true)
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            currentPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                           Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

                            //DebugUtil.debugYellow(currentPosition.x + " | " + currentPosition.y);

                            //DebugUtil.debugGreen(index.x + " | " + index.y);
                            Clean_CleanAtIndex(index.x, index.y, Get_DirectionOfMouse(currentPosition, firstPostion));
                            ItemsManager.Instance.ResetButton(ItemsType.NONE_ITEM);
                            isCanSwipe = false;

                        }

                    }
                    break;
                }


            case ItemsType.HAMMER_ITEM:
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2Int index = GetIndex_GetIndexFromMousePosition();
                    Hammer_ClearBlock(index.x, index.y);
                    ItemsManager.Instance.ResetButton(ItemsType.NONE_ITEM);
                }
                break;

            case ItemsType.CLOCK_ITEM:
                {
                    Reset_BoardStateByClockItem();
                    DebugUtil.debugYellow("CLOCK ITEM CLICKED");
                    ItemsManager.Instance.ResetButton(ItemsType.NONE_ITEM);
                }
                break;

            default:
                break;
        }
    }


    public GameObject[,] Grid
    {
        get
        {
            return grid;
        }

        set
        {
            grid = value;
        }
    }

    public void Implementation_BoardAction()
    {


        List<int> listRowDelete = new List<int>();
        List<int> listColDelete = new List<int>();

        for (int x = 0; x < gridHeight; x++)
        {
            if (Check_IsCanDeleteRowAtIndex(x))
            {
                listRowDelete.Add(x);
            }
        }

        for (int y = 0; y < gridWidth; y++)
        {
            if (Check_IsCanDeleteColumAtIndex(y))
            {
                listColDelete.Add(y);
            }
        }


        clearBlock.Clear();
        foreach (int rowIndex in listRowDelete)
        {
            Delete_RowAtIndex(rowIndex);
            MissionManager.Instance.CheckMission(TargetType.EatBlockLine, Line.Row); // --- 
            DebugUtil.debugGreen(grid[rowIndex, 1].GetComponent<Block>().type + " ");
        }

        foreach (int colIndex in listColDelete)
        {
            Delete_ColumnAtindex(colIndex);
            DebugUtil.debugGreen(grid[1, colIndex].GetComponent<Block>().type + " ");
            MissionManager.Instance.CheckMission(TargetType.EatBlockLine, Line.Collum); // ---
        }

        if (listColDelete.Count + listRowDelete.Count > 0)
            sfxSounds.playClearBlockSFX();

        // -------------------
        if (MissionManager.Instance.IsStep)
        {
            UIManager.Instance.Step -= 1;
        }
        if (listColDelete.Count == 0 && listRowDelete.Count == 0 && MissionManager.Instance.IsHaveGrass)
        {
            BoardGameManager.Instance.spreadGrass();
        }
        if (listColDelete.Count + listRowDelete.Count > 1)
        {
            MissionManager.Instance.CheckMission(TargetType.EatCombo, listColDelete.Count + listRowDelete.Count);
        }
        // -----------------
        if (clearBlock.Count > 0)
        {
            UIManager.Instance.Score += (listColDelete.Count * 100 + listRowDelete.Count * 100) * (listRowDelete.Count + listColDelete.Count);
            UIManager.Instance.ShowAddScoreText(clearBlock);
        }
        else
        {
            UIManager.Instance.Score += addedBlock.Count;
            MissionManager.Instance.CheckMission(TargetType.Scored, addedBlock.Count);
            try
            {
                UIManager.Instance.ShowAddScoreText(addedBlock);
            }
            catch (Exception)
            {
                DebugUtil.debugError("Failed to show add score");
            }
        }

        MissionManager.Instance.CheckMission(TargetType.Scored, (listColDelete.Count * 100 + listRowDelete.Count * 100) * (listRowDelete.Count + listColDelete.Count)); // ---

        isGameOver = Check_IsGameOver();

        if (isGameOver)
        {
            UIManager.Instance.GameOver(true);
        }


    }


    #region CREATE BOARD GAME
    public void Create_Grid()
    {
        Grid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject newBlock = GameObject.Instantiate(block, container);
                Grid[x, y] = newBlock;
                newBlock.GetComponent<Block>().ChangeType(BlockType.BLOCK_BOARD);
                newBlock.GetComponent<Block>().index = new Vector2Int(x, y);
                newBlock.layer = LayerMask.NameToLayer("BOARD_LAYER");
                //newBlock.layer = LayerMask.NameToLayer("Board");
            }
        }
    }
    #endregion


    //-------------------------------------------------------------------------------------------------------
    #region DELETE BLOCK
    public bool Check_IsCanDeleteRowAtIndex(int rowIndex)
    {
        for (int colIndex = 0; colIndex < gridWidth; colIndex++)
        {
            if (grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_BOARD
                || grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_ROCK
                || grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                return false;
        }
        return true;
    }

    public bool Check_IsCanDeleteColumAtIndex(int columnIndex)
    {
        for (int rowIndex = 0; rowIndex < gridHeight; rowIndex++)
        {
            if (grid[rowIndex, columnIndex].GetComponent<Block>().type == BlockType.BLOCK_BOARD
                || grid[rowIndex, columnIndex].GetComponent<Block>().type == BlockType.BLOCK_ROCK
                || grid[rowIndex, columnIndex].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                return false;
        }
        return true;
    }

    public void Delete_RowAtIndex(int rowIndex)
    {
        Save_PreviousStepChangedBoard(ActionOnBoard.DELETE_SHAPE_AT_ROW, new Vector2Int(rowIndex, 0));
        for (int colIndex = 0; colIndex < gridWidth; colIndex++)
        {
            if (!grid[rowIndex, colIndex].GetComponent<Block>().IsClearing)
            {
                CheckMissionAndDeleteLine(grid[rowIndex, colIndex].GetComponent<Block>());
                try
                {
                    if (grid[rowIndex + 1, colIndex].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                    {
                        if (!(grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_BLUE && grid[rowIndex + 1, colIndex].GetComponent<Block>().IceBreak != 0))
                        {
                            grid[rowIndex + 1, colIndex].GetComponent<Block>().ClearBlock();
                            MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, BlockType.BLOCK_GRASS, 1);
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {

                }
                try
                {
                    if (grid[rowIndex - 1, colIndex].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                    {
                        if (!(grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_BLUE && grid[rowIndex + 1, colIndex].GetComponent<Block>().IceBreak != 0))
                        {
                            grid[rowIndex - 1, colIndex].GetComponent<Block>().ClearBlock();
                            MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, BlockType.BLOCK_GRASS, 1);
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {

                }
                clearBlock.Add(grid[rowIndex, colIndex]);
            }
        }

    }

    public void Delete_ColumnAtindex(int colIndex)
    {
        Save_PreviousStepChangedBoard(ActionOnBoard.DELETE_SHAPE_AT_COLUMN, new Vector2Int(0, colIndex));

        for (int rowIndex = 0; rowIndex < gridHeight; rowIndex++)
        {
            if (!grid[rowIndex, colIndex].GetComponent<Block>().IsClearing)
            {
                CheckMissionAndDeleteLine(grid[rowIndex, colIndex].GetComponent<Block>());
                try
                {
                    if (grid[rowIndex, colIndex + 1].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                    {
                        if (!(grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_BLUE && grid[rowIndex + 1, colIndex].GetComponent<Block>().IceBreak != 0))
                        {
                            grid[rowIndex, colIndex + 1].GetComponent<Block>().ClearBlock();
                            MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, BlockType.BLOCK_GRASS, 1);
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    //Index out of range exception
                    DebugUtil.debugGreen("Index out of range");
                }
                try
                {
                    if (grid[rowIndex, colIndex - 1].GetComponent<Block>().type == BlockType.BLOCK_GRASS)
                    {
                        if (!(grid[rowIndex, colIndex].GetComponent<Block>().type == BlockType.BLOCK_BLUE && grid[rowIndex + 1, colIndex].GetComponent<Block>().IceBreak != 0))
                        {
                            grid[rowIndex, colIndex - 1].GetComponent<Block>().ClearBlock();
                            MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, BlockType.BLOCK_GRASS, 1);
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {

                }
                clearBlock.Add(grid[rowIndex, colIndex]);
            }

        }
    }

    public void CheckMissionAndDeleteLine(Block block)
    {
        if (block.type != BlockType.BLOCK_BLUE)
        {
            MissionManager.Instance.CheckMission(TargetType.EatBlockColor, block.type, 1);
            block.ClearBlock();
        }
        else
        {
            if (block.IceBreak == 0)
            {
                MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, block.type, 1);
                block.ClearBlock();  
            }
            else
                block.UpdateIceBreak();
        }

    }
    #endregion

    #region ADD BLOCK TO BORD
    public void Add_BlockToBoard(int x, int y, BlockType typeOfBlock)
    {
        if (typeOfBlock != BlockType.EMPTY)
        {
            if (Check_IsValidBlock(x, y))
            {
                grid[x, y].GetComponent<Block>().ChangeType(typeOfBlock);
            }
        }

    }

    public void Add_ShapeToBoard(int startX, int startY, GameObject shape)
    {
        sfxSounds.playPlaceBlockSFX();

        Save_PreviousStepChangedBoard(ActionOnBoard.ADD_SHAPE,new Vector2Int(startX,startY),shape);
        addedBlock.Clear();
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int rowIndex = startX + x;
                int colIndex = startY + y;

                BlockType typeOfShape = shape.GetComponent<BlockShape>().blocks[x, y].GetComponent<Block>().type;
                if (typeOfShape != BlockType.EMPTY)
                {
                    Add_BlockToBoard(rowIndex, colIndex, typeOfShape);
                    addedBlock.Add(grid[rowIndex, colIndex]);
                    Debug.Log("ADD COMPLETED");
                }
            }
        }
        MissionManager.Instance.CheckMission(TargetType.PutBlockShape, shape.GetComponent<BlockShape>().Shape);
    }

    #endregion

    #region CHECK POSITION

    public bool CheckValidPosition_ShapePosition(int startX, int startY, GameObject shape)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int rowIndex = startX + x;
                int colIndex = startY + y;
                if (shape.GetComponent<BlockShape>().blocks[x, y].GetComponent<Block>().type != BlockType.EMPTY)
                {
                    if (!Check_IsValidBlock(rowIndex, colIndex))
                    {
                        //Debug.Log( rowIndex +  "," + colIndex + ":INVALID POSITION");
                        return false;
                    }
                }

            }
        }
        return true;
    }

    public bool CheckValidPosition_ShapePosition(int startX, int startY, BlockShape shape)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                int rowIndex = startX + x;
                int colIndex = startY + y;

                if (shape.blocks[x, y].GetComponent<Block>().type != BlockType.EMPTY)
                {
                    if (!Check_IsValidBlock(rowIndex, colIndex))
                    {
                        //Debug.Log( rowIndex +  "," + colIndex + ":INVALID POSITION");
                        return false;
                    }
                }

            }
        }
        return true;
    }
    public bool Check_IsValidBlock(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight)
        {
            return false;
        }

        BlockType typeOfGrid = grid[x, y].GetComponent<Block>().type;
        if (typeOfGrid != BlockType.BLOCK_BOARD)
        {
            return false;
        }

        return true;
    }

    public bool Check_IsCanSaveBlock(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight)
        {
            return false;
        }

        BlockType typeOfGrid = grid[x, y].GetComponent<Block>().type;
        if (typeOfGrid == BlockType.BLOCK_BOARD)
        {
            return false;
        }

        return true;
    }


    public bool Check_IsValidBlock(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= gridWidth || pos.y >= gridHeight)
        {
            return false;
        }

        if (grid[pos.x, pos.y].GetComponent<Block>().type != BlockType.BLOCK_BOARD)
        {
            return false;
        }

        return true;
    }


    public bool Check_isValidPosition(int x, int y)
    {

        return x >= 0 && y >= 0 && x < gridWidth && y < gridHeight;
    }
    #endregion

    #region GAME OVER

    public bool Check_IsEmptyShape(GameObject shape)
    {
        if (shape.GetComponent<BlockShape>().Block == BlockType.EMPTY)
            return true;
        return false;
    }


    public bool Check_IsStillPutShape(GameObject shape)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (CheckValidPosition_ShapePosition(x, y, shape))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool Check_IsStillPutShape(BlockShape shape)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (CheckValidPosition_ShapePosition(x, y, shape))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool Check_IsGameOver()
    {

        if (UIManager.Instance.BlockShape_4x4_1.Block == BlockType.EMPTY &&
          UIManager.Instance.BlockShape_4x4_2.Block == BlockType.EMPTY &&
          UIManager.Instance.BlockShape_4x4_3.Block == BlockType.EMPTY)
        {
            DebugUtil.debugYellow("ALL BLOCK EMPTY");
            UIManager.Instance.RandomHard();
            return false;
        }


        if (UIManager.Instance.BlockShape_4x4_1.Block != BlockType.EMPTY)
        {
            DebugUtil.debugYellow(count + "BlockShape_4x4_1  NOT EMPTY");
            if (Check_IsStillPutShape(UIManager.Instance.BlockShape_4x4_1))
            {
                DebugUtil.debugYellow(count + "BlockShape_4x4_1 CAN PUT");

                return false;
            }
        }


        if (UIManager.Instance.BlockShape_4x4_2.Block != BlockType.EMPTY)
        {
            DebugUtil.debugYellow(count + "BlockShape_4x4_2  NOT EMPTY");

            if (Check_IsStillPutShape(UIManager.Instance.BlockShape_4x4_2))
            {
                DebugUtil.debugYellow(count + "BlockShape_4x4_2 CAN PUT");

                return false;
            }
        }

        if (UIManager.Instance.BlockShape_4x4_3.Block != BlockType.EMPTY)
        {
            DebugUtil.debugYellow(count + "BlockShape_4x4_3  NOT EMPTY");
            if (Check_IsStillPutShape(UIManager.Instance.BlockShape_4x4_3))
            {
                DebugUtil.debugYellow(count + "BlockShape_4x4_3 CAN PUT");
                return false;
            }
        }

        return true;
    }

    #endregion

    #region RESET BOARD

    public void Reset_Board()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].GetComponent<Block>().ChangeType(BlockType.BLOCK_BOARD);
            }
        }
    }
    #endregion

    #region ITEM EVENTS
    public bool Check_IsValidBlockForBreak(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight)
        {
            return false;
        }

        BlockType typeOfGrid = grid[x, y].GetComponent<Block>().type;
        if (typeOfGrid == BlockType.BLOCK_BOARD)
        {
            return false;
        }

        return true;
    }

    public void Clean_CleanAtIndex(int x, int y, TypeOfClean typeOfClean)
    {
        switch (typeOfClean)
        {
            case TypeOfClean.CLEAN_COLUM:
                Delete_ColumnAtindex(y);
                break;

            case TypeOfClean.CLEAN_ROW:
                Delete_RowAtIndex(x);
                break;

            default:
                break;
        }
    }

    public void Hammer_ClearBlock(int x, int y)
    {
        Save_PreviousStepChangedBoard(ActionOnBoard.DELETE_BY_USING_HAMER, new Vector2Int(x, y));
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int rowIndex = x + i;
                int colIndex = y + j;

                // DebugUtil.debugYellow(rowIndex + " | " + colIndex);
                if (Check_IsValidBlockForBreak(rowIndex, colIndex))
                {
                    if (grid[rowIndex, colIndex].GetComponent<Block>().type != BlockType.BLOCK_BOARD)
                    {
                        if (!grid[rowIndex, colIndex].GetComponent<Block>().IsClearing)
                            grid[rowIndex, colIndex].GetComponent<Block>().ClearBlock();
                    }

                }
            }
        }
    }
    #endregion

    #region MOUSE IMPLEMENTATION
    public LayerMask castLayer;

    Vector2Int GetIndex_GetIndexFromMousePosition()
    {

        Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                     Camera.main.ScreenToWorldPoint(Input.mousePosition).y);


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 200f, castLayer);
        Vector2Int res = new Vector2Int();

        if (hit.collider != null)
        {
            res = hit.collider.GetComponent<Block>().index;
        }
        return res;
    }

    TypeOfClean Get_DirectionOfMouse(Vector2 startDragPosition, Vector2 endDragPosition)
    {

        float deltaDistanceX = endDragPosition.x - startDragPosition.x;
        float deltaDistanceY = endDragPosition.y - startDragPosition.y;

        if ((Mathf.Abs(deltaDistanceX) >= Mathf.Abs(deltaDistanceY)))
        {
            return TypeOfClean.CLEAN_ROW;
        }

        return TypeOfClean.CLEAN_COLUM;
    }
    #endregion


    #region IMPLEMENTATION FILE ITEMS DATA

    public void Save_PreviousStepChangedBoard(ActionOnBoard action, Vector2Int index)
    {
        switch (action)
        {
            case ActionOnBoard.DELETE_BY_USING_HAMER:
                {
                    List<BlockStateOnBoard> blocks = new List<BlockStateOnBoard>();

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int rowIndex = index.x + i;
                            int colIndex = index.y + j;

                            if (Check_IsCanSaveBlock(rowIndex, colIndex))
                            {
                                BlockType typeOfBlock = grid[rowIndex, colIndex].GetComponent<Block>().type;
                                if (typeOfBlock != BlockType.BLOCK_BOARD)
                                {
                                    blocks.Add(new BlockStateOnBoard(new Vector2Int(rowIndex, colIndex), typeOfBlock));
                                }
                            }
                            
                        }
                    }

                    if (listOfPreviousStepOnBoard.Count >= ItemAmount.MAXIMUM_OF_CLOCK_ITEM)
                    {
                        listOfPreviousStepOnBoard.RemoveAt(0);
                    }
                    listOfPreviousStepOnBoard.Add(new HistoryStateOnBoard(action, blocks));
                }
                break;

            case ActionOnBoard.DELETE_SHAPE_AT_COLUMN:
                {
                    List<BlockStateOnBoard> blocks = new List<BlockStateOnBoard>();
                    for (int rowIndex = 0; rowIndex < gridHeight; rowIndex++)
                    {
                        BlockType typeOfBlock = grid[rowIndex, index.y].GetComponent<Block>().type;
                        if (typeOfBlock != BlockType.BLOCK_BOARD)
                        {
                            blocks.Add(new BlockStateOnBoard(new Vector2Int(rowIndex, index.y), typeOfBlock));
                        }
                    }

                    if (listOfPreviousStepOnBoard.Count >= ItemAmount.MAXIMUM_OF_CLOCK_ITEM)
                    {
                        listOfPreviousStepOnBoard.RemoveAt(0);
                    }
                    listOfPreviousStepOnBoard.Add(new HistoryStateOnBoard(action, blocks));

                }
                break;

            case ActionOnBoard.DELETE_SHAPE_AT_ROW:
                {
                    List<BlockStateOnBoard> blocks = new List<BlockStateOnBoard>();
                    for (int colIndex = 0; colIndex < gridHeight; colIndex++)
                    {
                        BlockType typeOfBlock = grid[index.x, colIndex].GetComponent<Block>().type;
                        if (typeOfBlock != BlockType.BLOCK_BOARD)
                        {
                            blocks.Add(new BlockStateOnBoard(new Vector2Int(index.x, colIndex), typeOfBlock));
                        }
                    }
                    //the maxcimumcapacity of list is 10 state
                    //if (listOfPreviousStepOnBoard.Count >= ItemAmount.MAXIMUM_OF_CLOCK_ITEM)
                    //{
                    //    listOfPreviousStepOnBoard.Pop();
                    //}
                    if (listOfPreviousStepOnBoard.Count >= ItemAmount.MAXIMUM_OF_CLOCK_ITEM)
                    {
                        listOfPreviousStepOnBoard.RemoveAt(0);
                    }
                    listOfPreviousStepOnBoard.Add(new HistoryStateOnBoard(action, blocks));
                }
                break;

            default:
                break;
        }

    }

    public void Save_PreviousStepChangedBoard(ActionOnBoard action, Vector2Int startPosition, GameObject shape)
    {
        if (action == ActionOnBoard.ADD_SHAPE)
        {
            if (listOfPreviousStepOnBoard.Count >= ItemAmount.MAXIMUM_OF_CLOCK_ITEM)
            {
                listOfPreviousStepOnBoard.RemoveAt(0);
            }
            listOfPreviousStepOnBoard.Add(new HistoryStateOnBoard(action, startPosition, shape));
        }

    }

    //Reset board by using clock item
    public void Reset_BoardStateByClockItem()
    {
        if (listOfPreviousStepOnBoard.Count > 0)
        {


            HistoryStateOnBoard previousState = listOfPreviousStepOnBoard[listOfPreviousStepOnBoard.Count -1];
            listOfPreviousStepOnBoard.RemoveAt(listOfPreviousStepOnBoard.Count - 1);

            switch (previousState.action)
            {
                case ActionOnBoard.ADD_SHAPE:
                    foreach (BlockStateOnBoard block in previousState.blocks)
                    {
                        grid[block.index.x, block.index.y].GetComponent<Block>().ChangeType(BlockType.BLOCK_BOARD);
                    }
                    break;

                case ActionOnBoard.DELETE_SHAPE_AT_COLUMN:
                    foreach (BlockStateOnBoard block in previousState.blocks)
                    {
                        grid[block.index.x, block.index.y].GetComponent<Block>().ChangeType(block.type);
                    }
                    break;

                case ActionOnBoard.DELETE_SHAPE_AT_ROW:
                    foreach (BlockStateOnBoard block in previousState.blocks)
                    {
                        grid[block.index.x, block.index.y].GetComponent<Block>().ChangeType(block.type);
                    }
                    break;

                case ActionOnBoard.DELETE_BY_USING_HAMER:
                    foreach (BlockStateOnBoard block in previousState.blocks)
                    {
                        grid[block.index.x, block.index.y].GetComponent<Block>().ChangeType(block.type);
                    }
                    break;

                default:
                    break;
            }
        }

    }
    #endregion

    #region SPREDING
    public void spreadGrass(float TimeDelay = 0.03f)
    {
        System.Random random = new System.Random();
        bool isSpreaded = false;

        List<BlockImpedient> lGrass = new List<BlockImpedient>();
        lGrass.AddRange(MissionManager.Instance.lGrass);

        while (!isSpreaded && lGrass.Count > 0)
        {
            int choose = random.Next(0, lGrass.Count);
            int[] pos = lGrass[choose].PosInMap;

            List<int[]> lSpreadGrass = new List<int[]>();

            lSpreadGrass.Add(new int[] { pos[0] + 1, pos[1] });
            lSpreadGrass.Add(new int[] { pos[0] - 1, pos[1] });
            lSpreadGrass.Add(new int[] { pos[0], pos[1] + 1 });
            lSpreadGrass.Add(new int[] { pos[0], pos[1] - 1 });

            while (!isSpreaded && lSpreadGrass.Count > 0)
            {
                int index = random.Next(0, lSpreadGrass.Count);

                isSpreaded = CanSpreading(lSpreadGrass[index]);
                lSpreadGrass.RemoveAt(index);
            }
            lGrass.RemoveAt(choose);
        }
    }
    private bool CanSpreading(int[] pos)
    {
        try
        {
            if (grid[pos[0], pos[1]].GetComponent<Block>().type == BlockType.BLOCK_GREEN  ||
                grid[pos[0], pos[1]].GetComponent<Block>().type == BlockType.BLOCK_ORANGE ||
                grid[pos[0], pos[1]].GetComponent<Block>().type == BlockType.BLOCK_PURPLE ||
                grid[pos[0], pos[1]].GetComponent<Block>().type == BlockType.BLOCK_RED    ||
                grid[pos[0], pos[1]].GetComponent<Block>().type == BlockType.BLOCK_YELLOW &&
                pos[0] > 0 && pos[0] < gridHeight && pos[1] > 0 && pos[1] < gridWidth)
            {
                grid[pos[0], pos[1]].GetComponent<Block>().ChangeType(BlockType.BLOCK_GRASS);

                MissionManager.Instance.lGrass.Add(new BlockImpedient(BlockType.BLOCK_GRASS, pos));
                MissionManager.Instance.CheckMission(TargetType.DestroyImpedient, BlockType.BLOCK_GRASS, -1);

                return true;
            }
        }
        catch (IndexOutOfRangeException e)
        {

        }
        return false;
    }
    #endregion
}
