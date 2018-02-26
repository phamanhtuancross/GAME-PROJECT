using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : Singleton<MissionController>
{

    public List<Mission> lMission = new List<Mission>();
    private Dictionary<int, Mission> lMissionLevel = null;

    public Dictionary<int, Mission> LMissionLevel
    {
        get
        {
            if (lMissionLevel != null)
                return lMissionLevel;
            else
            {
                lMissionLevel = initListMissionData();
                return lMissionLevel;
            }
        }
    }

    // init List MissionData
    private Dictionary<int, Mission> initListMissionData()
    {
        Dictionary<int, Mission> result = new Dictionary<int, Mission>();
        #region level 1-14 Easy
        result.Add(1, new Mission(1
            , new List<Target> { new Target(TargetType.Scored, 10) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(2, new Mission(2
            , new List<Target> { new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(3, new Mission(3
            , new List<Target> { new Target(TargetType.Scored, 1000), new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 8) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(4, new Mission(4
            , new List<Target> { new Target(TargetType.Scored, 1000), new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_PURPLE, 8) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(5, new Mission(5
            , new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 8) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(6, new Mission(6
            , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.L2, 3), new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(7, new Mission(7
            , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.LINE2, 3), new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 3), new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(8, new Mission(8
            , new List<Target> { new Target(TargetType.EatBlockLine, Line.Row, 3), new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(9, new Mission(9
            , new List<Target> { new Target(TargetType.EatBlockLine, Line.Row, 3), new Target(TargetType.EatBlockLine, Line.Collum, 3), new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(10, new Mission(10
            , new List<Target> { new Target(TargetType.EatCombo, 2, 3), new Target(TargetType.Scored, 1000) }
            , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(11, new Mission(11
           , new List<Target> { new Target(TargetType.Scored, 2000) }
           , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 60, 0));
        result.Add(12, new Mission(12
           , new List<Target> { new Target(TargetType.Scored, 2000), new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_PURPLE, 8) }
           , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 60, 0));
        result.Add(13, new Mission(13
           , new List<Target> { new Target(TargetType.Scored, 1000) }
           , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 180));
        result.Add(14, new Mission(14
           , new List<Target> { new Target(TargetType.Scored, 2000) }
           , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 300));

        #endregion
        #region level 15-29 Normal
        result.Add(15, new Mission(15, new List<Target> { new Target(TargetType.Scored, 5000) }, new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 0));
        result.Add(16, new Mission(16, new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.L3, 10) }
          , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(17, new Mission(17, new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE1, 10), new Target(TargetType.PutBlockShape, ShapeType.LINE3, 10) }
         , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(18, new Mission(18, new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 20), new Target(TargetType.EatBlockLine, Line.Row, 20) }
         , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 0));
        result.Add(19, new Mission(19, new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 50), new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 50), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 50) }
         , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Hard, 100, 0));
        result.Add(20, new Mission(20, new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 30), new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 10), new Target(TargetType.PutBlockShape, ShapeType.SQUARE3, 5) }
        , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 6}) }, 0, Difficult.Normal, 0, 0));
        result.Add(21, new Mission(21, new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 30), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 30), new Target(TargetType.EatBlockColor, BlockType.BLOCK_GREEN, 30) }
       , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 6}), new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 4}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 5 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 5}) }, 0, Difficult.Normal, 0, 0));
        result.Add(22, new Mission(22, new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.LINE2, 10), new Target(TargetType.PutBlockShape, ShapeType.LINE3, 10), new Target(TargetType.PutBlockShape, ShapeType.LINE4, 10) }
       , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 6}), new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 4}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 5 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 5}) }, 0, Difficult.Normal, 0, 0));
        result.Add(23, new Mission(23, new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.L2, 20), new Target(TargetType.PutBlockShape, ShapeType.L3, 20) }
        , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 0));
        result.Add(24, new Mission(24, new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE3, 10) }
        , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 0));
        result.Add(25, new Mission(25, new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 60), new Target(TargetType.EatBlockColor, BlockType.BLOCK_PURPLE, 60) }
        , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Hard, 80, 0));
        result.Add(26, new Mission(26, new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 30), new Target(TargetType.EatBlockLine, Line.Row, 30) }
       , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Easy, 0, 0));
        result.Add(27, new Mission(27, new List<Target> { new Target(TargetType.Scored, 5000) }
       , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 420));
        result.Add(28, new Mission(28, new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 50) }
       , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Normal, 0, 0));
        result.Add(29, new Mission(29, new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 60), new Target(TargetType.EatBlockLine, Line.Row, 60) }
       , new List<Item> { }, new List<BlockImpedient> { }, 0, Difficult.Hard, 0, 420));

        #endregion
        #region level 30 - 49 Have Rock
        result.Add(30, new Mission(30
            , new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 8), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 8) }
            , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 }) }, 0, Difficult.Easy, 0, 0));
        result.Add(31, new Mission(31
           , new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 30), new Target(TargetType.EatBlockColor, BlockType.BLOCK_ORANGE, 30), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 30) }
           , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 }) }, 0, Difficult.Normal, 0, 0));
        result.Add(32, new Mission(32
           , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 10), new Target(TargetType.PutBlockShape, ShapeType.L2, 10), new Target(TargetType.PutBlockShape, ShapeType.LINE2, 10) }
           , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 }) }, 0, Difficult.Normal, 0, 0));
        result.Add(33, new Mission(33
   , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 10), new Target(TargetType.PutBlockShape, ShapeType.L2, 20), new Target(TargetType.Scored, 5000) }
   , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 6}), new BlockImpedient(BlockType.BLOCK_GREEN, new int[] {3, 4}), new BlockImpedient(BlockType.BLOCK_ORANGE, new int[] {3, 5 }),
           new BlockImpedient(BlockType.BLOCK_RED, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_YELLOW, new int[] {6, 5}),  new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 }) }, 0, Difficult.Normal, 0, 0));
        result.Add(34, new Mission(34
           , new List<Target> { new Target(TargetType.Scored, 5000) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 6}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 4}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 5 }),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 5}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 })}, 0, Difficult.Normal, 0, 0));
        result.Add(35, new Mission(35
            , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 10), new Target(TargetType.PutBlockShape, ShapeType.SQUARE3, 5) }
            , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 2, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 3 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 6 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 7, 7 })}, 0, Difficult.Hard, 0, 0));
        result.Add(36, new Mission(36
           , new List<Target> { new Target(TargetType.PutBlockShape, ShapeType.SQUARE2, 10), new Target(TargetType.PutBlockShape, ShapeType.SQUARE3, 5) }
           , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 5 }),
             new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 3 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 6 }),
            }, 0, Difficult.Normal, 0, 0));
        result.Add(37, new Mission(37
           , new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 20) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 5 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 5 })}, 0, Difficult.Normal, 0, 0));
        result.Add(38, new Mission(38
           , new List<Target> { new Target(TargetType.EatBlockLine, Line.Row, 20) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 6 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 3 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 3 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 6 })}, 0, Difficult.Normal, 0, 0));
        result.Add(39, new Mission(39
           , new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 20), new Target(TargetType.EatBlockLine, Line.Row, 20) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 5 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 5 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 6}),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 5})}, 0, Difficult.Hard, 0, 0));
        result.Add(40, new Mission(40
           , new List<Target> { new Target(TargetType.EatCombo, 2, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Easy, 0, 0));
        result.Add(41, new Mission(41
           , new List<Target> { new Target(TargetType.EatCombo, 2, 10), new Target(TargetType.PutBlockShape, ShapeType.LINE4, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 0));
        result.Add(42, new Mission(42
           , new List<Target> { new Target(TargetType.EatCombo, 2, 10), new Target(TargetType.EatBlockColor, BlockType.BLOCK_PURPLE, 50) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 0));
        result.Add(43, new Mission(43
           , new List<Target> { new Target(TargetType.EatCombo, 2, 20) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 420));
        result.Add(44, new Mission(44
           , new List<Target> { new Target(TargetType.EatCombo, 3, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 0));
        result.Add(45, new Mission(45
           , new List<Target> { new Target(TargetType.EatBlockColor, BlockType.BLOCK_RED, 35), new Target(TargetType.EatBlockColor, BlockType.BLOCK_YELLOW, 35), new Target(TargetType.EatBlockColor, BlockType.BLOCK_PURPLE, 35) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 60, 0));
        result.Add(46, new Mission(46
           , new List<Target> { new Target(TargetType.EatBlockLine, Line.Collum, 20), new Target(TargetType.EatBlockLine, Line.Row, 20), new Target(TargetType.EatCombo, 2, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 5, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 4 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 5 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 4, 5 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {3, 6 }),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 3}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 6}),
           new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 4}), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] {6, 5})}, 0, Difficult.Normal, 0, 0));
        result.Add(47, new Mission(47
           , new List<Target> { new Target(TargetType.EatCombo, 3, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 0));
        result.Add(48, new Mission(48
           , new List<Target> { new Target(TargetType.EatCombo, 3, 10) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Normal, 0, 0));
        result.Add(49, new Mission(49
           , new List<Target> { new Target(TargetType.Scored, 10000) }
           , new List<Item> { }, new List<BlockImpedient> {new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 7 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 2 }), new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 6, 2 }),
            new BlockImpedient(BlockType.BLOCK_ROCK, new int[] { 3, 7 })}, 0, Difficult.Hard, 100, 0));
        #endregion
        #region lv 50 - 69 Ice Block
        result.Add(50, new Mission(50
            , new List<Target> { new Target(TargetType.DestroyImpedient, BlockType.BLOCK_BLUE, 2) }
            , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_BLUE, new int[] { 4, 2 }), new BlockImpedient(BlockType.BLOCK_BLUE, new int[] { 4, 7 }) }, 0, Difficult.Easy, 0, 0));
        #endregion
        #region level 70 - 80 Grass Block
        result.Add(51, new Mission(51
           , new List<Target> { new Target(TargetType.Scored, 1000) }
           , new List<Item> { }, new List<BlockImpedient> { new BlockImpedient(BlockType.BLOCK_GRASS, new int[] { 4, 4 }) }, 0, Difficult.Easy, 0, 0));
        #endregion 
        for (int i = 0; i < lMission.Count; i++)
        {
            if (lMission[i] != null)
                result.Add(result.Count + 1, lMission[i]);
        }
        return result;
    }
}
