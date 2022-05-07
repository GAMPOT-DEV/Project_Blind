using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define 클래스입니다. enum들을 관리합니다.
/// </summary>
public class Define
{
    public enum Scene
    {
        Unknown,
        GameScene,
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
    }
}