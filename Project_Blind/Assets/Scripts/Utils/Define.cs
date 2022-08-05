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
        황현택,
        황현택_dest,
        MainScene,
        김종현,
        TestFogOfWar
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
}