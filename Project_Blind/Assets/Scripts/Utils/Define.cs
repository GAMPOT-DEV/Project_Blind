using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define 클래스입니다. enum들을 관리합니다.
/// </summary>
public class Define
{
    public enum ObjectType
    {
        Item,
        Npc
    }
    public enum ObjectState
    {
        NonKeyDown,
        KeyDown,
        Ing
    }
    
    public enum Scene
    {
        Unknown,
        GameScene,
        MainScene,
        심연,
        절외부,
        숲,
        마을,
        고목,
        동굴,
        장산범방,
        

    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
        EndDrag,
        Enter,
        Exit,
    }
    public enum Language
    {
        KOR,
        ENG,
        MaxCount
    }

    public struct Resolution
    {
        public int width;
        public int height;
    }
    public enum AttackType
    {
        NonAttack,
        ParingImpossibleAttack,
        BaseAttack,
        PowerAttack
    }
    public enum KeyAction
    {
        Jump,
        DownJump,
        LeftMove,
        RightMove,
        Paring,
        Interaction,
        Attack,
        Dash,
        Wave,
        Horizontal,
        Vertical,
        Skill,
        ChangeSlot,
        ItemT,
        PowerAttack,
        ItemUsing
    }
    public enum ClueItem
    {
        Unknown=0,

        TestClue1 = 1,
        TestClue2 = 2,
        TestClue3 = 3,
        TestClue4 = 4,
        TestClue5 = 5,
        TestClue6 = 6,
        TestClue7 = 7,
    }

    public enum BagItem
    {
        Unknown = 0,

        TestItem1 = 1,
        TestItem2 = 2,
        TestItem3 = 3,
        TestItem4 = 4,
        TestItem5 = 5,
        TestItem6 = 6,
        TestItem7 = 7,
    }

    public enum TalismanItem
    {
        Unknown = 0,

        TestTalisman1 = 1,
        TestTalisman2 = 2,
        TestTalisman3 = 3,
        TestTalisman4 = 4,
        TestTalisman5 = 5,
        TestTalisman6 = 6,
        TestTalisman7 = 7,
    }
    public enum ScriptTitle
    {
        Unknown,

        Test1,
        Test2,
        Stage1,
        Stage2,
        Stage2_1,
        Stage2_2,
        Stage3,
        Stage4,
        Stage5,

        CutScene1,
        CutScene2,
        CutScene3,
        CutScene4,
        CutScene5,

        temple,

        village1,
        village2,
        village3
    }
    public enum TalismanAbility
    {
        STR,
        DEX,
    }
}

namespace Blind {
    public enum Facing
    {
        Right = 1,
        Left = -1
    }
}