using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidBlock : MonoBehaviour
{

    public static int NUMBER_BLOCK_CREATE = 3;

    /// <summary>
    /// Calculate Random Easy
    /// </summary>
    /// <returns></returns>
    public static List<BlockShape.ShapeDirectionPair> AvoidBlockEasy()
    {
        System.Random random = new System.Random();
        List<BlockShape.ShapeDirectionPair> lBlockCreate = new List<BlockShape.ShapeDirectionPair>();
        for (int i = 0; i < NUMBER_BLOCK_CREATE; i++)
        {
            int temp = random.Next(0, BlockShape.LBlockShape.Count);
            lBlockCreate.Add(BlockShape.LBlockShape[temp]);
        }

        return lBlockCreate;
    }
    /// <summary>
    /// Hard Random Method
    /// </summary>
    /// <param name="grid"> Board </param>
    /// <returns></returns>
    public static List<BlockShape.ShapeDirectionPair> AvoidBlockHard(GameObject[,] grid)
    {
        Board board = new Board(grid);
        CalculateHardRandom calculateHardRandom = new CalculateHardRandom(board);

        return calculateHardRandom.CalculateBestChoice();
    }

    // Random Block Typoe
    public static List<BlockType> RandomBlockType()
    {
        System.Random random = new System.Random();
        List<BlockType> lBlockType = new List<BlockType>();
        do
        {
            lBlockType.Add((BlockType)random.Next((int)BlockType.BLOCK_GREEN, (int)BlockType.BLOCK_YELLOW + 1));
        } while (lBlockType.Count < 3);

        return lBlockType;
    }
    #region Calculate Hard Random
    protected class Board
    {
        private int[][] tiles;

        public int[][] Tiles
        {
            get
            {
                return tiles;
            }
        }

        public Board(GameObject[,] grid)
        {
            init(grid);
        }

        public Board(int[][] board)
        {
            CopyBoard(board);
        }

        private void init(GameObject[,] grid)
        {
            tiles = new int[BoardGameManager.gridHeight][];
            for (int x = 0; x < BoardGameManager.gridHeight; x++)
            {
                tiles[x] = new int[BoardGameManager.gridWidth];
                {
                    for (int y = 0; y < BoardGameManager.gridWidth; y++)
                    {
                        if (grid[x, y].GetComponent<Block>().type == BlockType.BLOCK_BOARD)
                            tiles[x][y] = 0;
                        else if (grid[x, y].GetComponent<Block>().type == BlockType.BLOCK_GRASS
                            || grid[x, y].GetComponent<Block>().type == BlockType.BLOCK_ROCK)
                            tiles[x][y] = 2;
                        else if (grid[x, y].GetComponent<Block>().type == BlockType.EMPTY)
                            tiles[x][y] = 3;
                        else
                            tiles[x][y] = 1;
                    }
                }
            }

            /*
            // o trong tren
            Tiles[0][0] = Tiles[0][1] = Tiles[0][8] = Tiles[0][9] = 2;
            Tiles[1][0] = Tiles[1][9] = 2;
            Tiles[2][0] = Tiles[2][9] = 2;
            // o trong duoi
            Tiles[9][0] = Tiles[9][1] = Tiles[9][8] = Tiles[9][9] = 2;
            Tiles[8][0] = Tiles[8][9] = 2;
            Tiles[7][0] = Tiles[7][9] = 2;
            */
        }
        //-----------------------***-----------------------------
        #region Change Matrix When Move
        public bool ChangeMatrix(ShapeType block, int pos_X, int pos_Y)
        {
            bool isCheck = false;
            try
            {
                switch (block)
                {
                    case ShapeType.Line2down:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Line2up:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = 1;
                            isCheck = true;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                        }
                        break;
                    case ShapeType.Line3down:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X][pos_Y + 2] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X][pos_Y + 2] = 1;
                            isCheck = true;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X, pos_Y + 2);
                        }
                        break;
                    case ShapeType.Line3up:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 2][pos_Y] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 2][pos_Y] = 1;
                            isCheck = true;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 2, pos_Y);
                        }
                        break;
                    case ShapeType.Line4down:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0
                            && Tiles[pos_X][pos_Y + 2] == 0 && tiles[pos_X][pos_Y + 3] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X][pos_Y + 2]
                                = tiles[pos_X][pos_Y + 3] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X, pos_Y + 2);
                            checkBreak(pos_X, pos_Y + 3);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Line4up:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0
                            && tiles[pos_X + 2][pos_Y] == 0 && tiles[pos_X + 3][pos_Y] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 2][pos_Y]
                                  = tiles[pos_X + 3][pos_Y] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 2, pos_Y);
                            checkBreak(pos_X + 3, pos_Y);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Square: // checked
                        if (tiles[pos_X][pos_Y] == 0)
                        {
                            tiles[pos_X][pos_Y] = 1;
                            checkBreak(pos_X, pos_Y);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Square2:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0
                            && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 1][pos_Y + 1] == 0) //cheked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] =
                                tiles[pos_X + 1][pos_Y] = tiles[pos_X + 1][pos_Y + 1] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 1, pos_Y + 1);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Square3:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X][pos_Y + 2] == 0
                          && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 1][pos_Y + 1] == 0 && tiles[pos_X + 1][pos_Y + 2] == 0
                          && tiles[pos_X + 2][pos_Y] == 0 && tiles[pos_X + 2][pos_Y + 1] == 0 && tiles[pos_X + 2][pos_Y + 2] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X][pos_Y + 2] =
                                tiles[pos_X + 1][pos_Y] = tiles[pos_X + 1][pos_Y + 1] = tiles[pos_X + 1][pos_Y + 2]
                                = tiles[pos_X + 2][pos_Y] = tiles[pos_X + 2][pos_Y + 1] = tiles[pos_X + 2][pos_Y + 2] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X, pos_Y + 2);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 1, pos_Y + 1);
                            checkBreak(pos_X + 1, pos_Y + 2);
                            checkBreak(pos_X + 2, pos_Y);
                            checkBreak(pos_X + 2, pos_Y + 1);
                            checkBreak(pos_X + 2, pos_Y + 2);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle3downleft:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X + 1][pos_Y] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X + 1][pos_Y] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X + 1, pos_Y);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle3downright:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X + 1][pos_Y + 1] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X + 1][pos_Y + 1] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X + 1, pos_Y + 1);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle3upleft:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 1][pos_Y + 1] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 1][pos_Y + 1] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 1, pos_Y + 1);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle3upright:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 1][pos_Y - 1] == 0) // checked
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 1][pos_Y - 1] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 1, pos_Y - 1);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle5downleft:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X][pos_Y + 2] == 0
                            && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 2][pos_Y] == 0)
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X][pos_Y + 2]
                                = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 2][pos_Y] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X, pos_Y + 2);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 2, pos_Y);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle5downright:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X][pos_Y + 1] == 0 && tiles[pos_X][pos_Y + 2] == 0
                            && tiles[pos_X + 1][pos_Y + 2] == 0 && tiles[pos_X + 2][pos_Y + 2] == 0)
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X][pos_Y + 1] = tiles[pos_X][pos_Y + 2]
                                = tiles[pos_X + 1][pos_Y + 2] = tiles[pos_X + 2][pos_Y + 2] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X, pos_Y + 1);
                            checkBreak(pos_X, pos_Y + 2);
                            checkBreak(pos_X + 1, pos_Y + 2);
                            checkBreak(pos_X + 2, pos_Y + 2);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle5upleft:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 2][pos_Y] == 0
                            && tiles[pos_X + 2][pos_Y + 1] == 0 && tiles[pos_X + 2][pos_Y + 2] == 0)
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 2][pos_Y]
                                = tiles[pos_X + 2][pos_Y + 1] = tiles[pos_X + 2][pos_Y + 2] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 2, pos_Y);
                            checkBreak(pos_X + 2, pos_Y + 1);
                            checkBreak(pos_X + 2, pos_Y + 2);
                            isCheck = true;
                        }
                        break;
                    case ShapeType.Triangle5upright:
                        if (tiles[pos_X][pos_Y] == 0 && tiles[pos_X + 1][pos_Y] == 0 && tiles[pos_X + 2][pos_Y] == 0
                            && Tiles[pos_X + 2][pos_Y - 1] == 0 && Tiles[pos_X + 2][pos_Y - 2] == 0)
                        {
                            tiles[pos_X][pos_Y] = tiles[pos_X + 1][pos_Y] = tiles[pos_X + 2][pos_Y]
                                = tiles[pos_X + 2][pos_Y - 1] = tiles[pos_X + 2][pos_Y - 2] = 1;
                            checkBreak(pos_X, pos_Y);
                            checkBreak(pos_X + 1, pos_Y);
                            checkBreak(pos_X + 2, pos_Y);
                            checkBreak(pos_X + 2, pos_Y - 1);
                            checkBreak(pos_X + 2, pos_Y - 2);
                            isCheck = true;
                        }
                        break;
                    default:
                        //Debug.Log("cannot have this type block");
                        break;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                //Debug.Log("cannot use this type block" + pos_X + "-" + pos_Y + block.ToString());
            }

            return isCheck;
        }
        #endregion
        //-------------------------------------------------------

        #region EatLine
        private void checkBreak(int posX, int posY)
        {
            bool isX = true;
            bool isY = true;
            for (int i = 0; i < BoardGameManager.gridHeight; i++)
            {
                if (Tiles[i][posY] == 0 || Tiles[i][posY] == 2)
                    isX = false;
                if (Tiles[posX][i] == 0 || Tiles[i][posY] == 2)
                    isY = false;
            }
            if (isX)
            {
                for (int i = 0; i < BoardGameManager.gridHeight; i++)
                {
                    if (Tiles[i][posY] == 1 || Tiles[i][posY] == 3)
                        Tiles[i][posY] = 0;
                }
            }
            if (isY)
            {
                for (int i = 0; i < BoardGameManager.gridWidth; i++)
                {
                    if (Tiles[posX][i] == 1 || Tiles[i][posY] == 3)
                        Tiles[posX][i] = 0;
                }
            }
        }
        #endregion

        public void CopyBoard(int[][] board)
        {
            tiles = new int[BoardGameManager.gridHeight][];
            for (int x = 0; x < BoardGameManager.gridHeight; x++)
            {
                tiles[x] = new int[BoardGameManager.gridWidth];
                {
                    for (int y = 0; y < BoardGameManager.gridWidth; y++)
                    {
                        tiles[x][y] = board[x][y];
                    }
                }
            }
        }
    }

    protected class CalculateHardRandom
    {
        private Board board;

        private int numShapeType;
        List<MoveBlock> CanMoveBlock;
        List<MoveBlock> CannotMove;


        public CalculateHardRandom(Board board)
        {
            this.board = board;
            this.numShapeType = Enum.GetNames(typeof(ShapeType)).Length;
            CannotMove = new List<MoveBlock>();
            CanMoveBlock = new List<MoveBlock>();
            CalculateBlockCanMove();
        }

        public List<BlockShape.ShapeDirectionPair> CalculateBestChoice()
        {

            if (CannotMove.Count == 0 || CannotMove.Count == numShapeType)
            {
                return AvoidBlock.AvoidBlockEasy();
            }
            else
            {
                List<List<BlockShape.ShapeDirectionPair>> listCaseLv1 = new List<List<BlockShape.ShapeDirectionPair>>();
                List<List<BlockShape.ShapeDirectionPair>> listCaseLv2 = new List<List<BlockShape.ShapeDirectionPair>>();
                List<BlockShape.ShapeDirectionPair> listBlock = new List<BlockShape.ShapeDirectionPair>();
                System.Random random = new System.Random();

                for (int i = 0; i < CanMoveBlock.Count; i++)
                {
                    Board clone = new Board(this.board.Tiles);
                    clone.ChangeMatrix(CanMoveBlock[i].Block, CanMoveBlock[i].PosMove[0], CanMoveBlock[i].PosMove[1]);
                    int[] PosMove = new int[2];
                    for (int j = 0; j < CannotMove.Count; j++)
                    {

                        if (CanMove(CannotMove[j].Block, clone, ref PosMove))
                        {
                            listBlock.Add(BlockShape.LBlockShape[(int)CannotMove[j].Block]);
                            clone.ChangeMatrix(CannotMove[j].Block, PosMove[0], PosMove[1]);
                        }

                        if (listBlock.Count > 1)
                        {
                            break;
                        }

                    }

                    switch (listBlock.Count)
                    {
                        case 1:
                            listBlock.Add(BlockShape.LBlockShape[(int)CanMoveBlock[i].Block]);
                            for (int k = 0; k < CanMoveBlock.Count; k++)
                            {
                                if (CanMove(CanMoveBlock[k].Block, clone, ref PosMove))
                                {
                                    listBlock.Add(BlockShape.LBlockShape[(int)CanMoveBlock[k].Block]);
                                    break;
                                }
                            }
                            for (int j = numShapeType - 1; j >= 0; j--)
                            {
                                if (CanMove((ShapeType)j, clone, ref PosMove))
                                {
                                    listBlock.Add(BlockShape.LBlockShape[j]);
                                    break;
                                }
                            }
                            listCaseLv1.Add(listBlock);
                            break;
                        case 2:
                            listBlock.Add(BlockShape.LBlockShape[(int)CanMoveBlock[i].Block]);
                            listCaseLv2.Add(listBlock);
                            break;
                    }
                    listBlock = new List<BlockShape.ShapeDirectionPair>();
                }

                if (listCaseLv2.Count > 0)
                {
                    //Debug.Log("type1:" + listCaseLv2.Count);
                    int test = random.Next(0, listCaseLv2.Count);
                    return listCaseLv2[test];
                }
                if (listCaseLv1.Count > 0)
                {
                    //Debug.Log("type2:" + listCaseLv2.Count);
                    return listCaseLv1[random.Next(0, listCaseLv1.Count)];
                }
                Debug.Log("type3");
                return AvoidBlock.AvoidBlockEasy();
            }

        }

        private void CalculateBlockCanMove()
        {
            int[] posMove = new int[2];
            MoveBlock item = new MoveBlock();
            for (int i = 0; i < numShapeType; i++)
            {
                if (CannotMove.Count + CanMoveBlock.Count == numShapeType)
                    break;
                item.Block = (ShapeType)i;
                item.PosMove = null;
                if (CannotMove.Contains(item))
                    continue;

                if (CanMove(item.Block, this.board, ref posMove))
                {
                    item.PosMove = posMove;
                    CanMoveBlock.Add(item);
                }
                else
                {
                    if (item.Block == ShapeType.Square2 || item.Block == ShapeType.Square3)
                    {

                    }
                    else if (item.Block == ShapeType.Line2down || item.Block == ShapeType.Line2up)
                    {
                        for (int j = (int)item.Block + 1; j < numShapeType; j++)
                        {
                            MoveBlock item2 = new MoveBlock();
                            item2.Block = (ShapeType)j;
                            item2.PosMove = null;
                            if (item2.Block == ShapeType.Line2down
                                || item2.Block == ShapeType.Line2up
                                || (item.Block == ShapeType.Line2up && (item2.Block == ShapeType.Line3down || item2.Block == ShapeType.Line4down))
                                || (item.Block == ShapeType.Line2down && (item2.Block == ShapeType.Line3up || item2.Block == ShapeType.Line4up)))
                                continue;
                            else
                            {
                                CannotMove.Add(item2);
                            }

                        }
                    }
                    else if (item.Block == ShapeType.Line3down || item.Block == ShapeType.Line3up)
                    {
                        for (int j = (int)item.Block; j < numShapeType; j++)
                        {
                            MoveBlock item2 = new MoveBlock();
                            item2.Block = (ShapeType)j;
                            item2.PosMove = null;
                            if ((item.Block == ShapeType.Line3up && item2.Block == ShapeType.Line4down
                                || item.Block == ShapeType.Line3down && item2.Block == ShapeType.Line4up)
                                || item2.Block == item.Block)
                                continue;
                            else
                            {
                                CannotMove.Add(item2);
                            }
                        }
                    }
                    else if (item.Block == ShapeType.Triangle3downright || item.Block == ShapeType.Triangle3downleft ||
                        item.Block == ShapeType.Triangle3upleft || item.Block == ShapeType.Triangle3upright)
                    {
                        for (int j = (int)ShapeType.Triangle3upleft; j < numShapeType; j++)
                        {
                            MoveBlock item2 = new MoveBlock();
                            item2.Block = (ShapeType)j;
                            item2.PosMove = null;
                            if (item.Block == item2.Block)
                                continue;
                            CannotMove.Add(item2);
                        }
                    }
                    else
                    {
                        for (int j = (int)ShapeType.Triangle5upleft; j < numShapeType; j++)
                        {
                            MoveBlock item2 = new MoveBlock();
                            item2.Block = (ShapeType)j;
                            item2.PosMove = null;
                            if (item.Block == item2.Block)
                                continue;
                            CannotMove.Add(item2);
                        }
                    }
                    CannotMove.Add(item);
                }
            }
        }
        bool CanMove(ShapeType block, Board check, ref int[] pos)
        {
            Board clone = new Board(check.Tiles);
            for (int i = BoardGameManager.gridHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < BoardGameManager.gridWidth; j++)
                {
                    if (clone.ChangeMatrix(block, i, j))
                    {
                        pos[0] = i;
                        pos[1] = j;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    protected struct MoveBlock
    {
        int[] posMove;
        ShapeType block;

        public int[] PosMove { get; set; }
        public ShapeType Block { get; set; }
    }
    protected enum ShapeType
    {
        Square,
        Line2up, Line2down, Square2,
        Line3up, Line3down, Line4up, Line4down,
        Triangle3upleft, Triangle3upright, Triangle3downleft, Triangle3downright, Square3,
        Triangle5upleft, Triangle5upright, Triangle5downleft, Triangle5downright
    }
    #endregion
}