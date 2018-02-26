using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum TypeOfClean
{
    CLEAN_ROW, CLEAN_COLUM
}

public enum ActionOnBoard
{
    ADD_SHAPE,
    DELETE_SHAPE_AT_ROW,
    DELETE_SHAPE_AT_COLUMN,
    DELETE_BY_USING_HAMER
}

public enum ShapeHolderIndex
{
    FIRST_INDEX,
    SECOND_INDEX,
    THREE_INDEX
}


class Define : Singleton<Define>
{
    public static int SIZE_OF_SHAPE = 4;
    public static int NUMBER_OF_SHAPE_HOLDER = 3;
    public static string BOARD_DATA = "BOARD_OLD_DATA";
}

class ItemAmount : Singleton<ItemAmount>
{
    public static int MAXIMUM_OF_BRUSHER_ITEM = 3;
    public static int MAXIMUM_OF_HAMMER_ITEM = 0;
    public static int MAXIMUM_OF_CLOCK_ITEM = 3;
}
