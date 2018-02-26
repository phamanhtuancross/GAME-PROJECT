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


class Define : Singleton<Define>
{
    public static int SIZE_OF_SHAPE = 4;
}

class ItemAmount : Singleton<ItemAmount>
{
    public static int MAXIMUM_OF_BRUSHER_ITEM = 10;
    public static int MAXIMUM_OF_HAMMER_ITEM = 10;
    public static int MAXIMUM_OF_CLOCK_ITEM = 10;
}
