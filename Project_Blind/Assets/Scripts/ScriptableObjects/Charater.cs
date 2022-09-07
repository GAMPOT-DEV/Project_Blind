using UnityEngine;

namespace Blind.ScriptableObjects
{
    public abstract class Character : ScriptableObject
    {
        [SerializeField] public float maxHp = 10f; 
    }
}