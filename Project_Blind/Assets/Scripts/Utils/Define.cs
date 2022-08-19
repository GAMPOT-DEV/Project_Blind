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
        김종현,
        TestFogOfWar,
        심연,
        절외부,
        숲,
        마을,
        고목,
        동굴,
        장산범입구,
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
}