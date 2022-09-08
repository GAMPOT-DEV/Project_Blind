using UnityEngine;

namespace Blind.ScriptableObjects
{
    [ CreateAssetMenu( fileName = "Player", menuName = "ScriptableObject/Player" )]
    public class PlayerCharacter : Character
    {
        [SerializeField] public float jumpSpeed = 1f;
        [SerializeField] public float jumpAbortSpeedReduction = 100f;
        [SerializeField] public float gravity = 30f;
        
        [SerializeField] public float maxSpeed = 5f;
        [SerializeField] public float groundAcceleration = 100f;
        [SerializeField] public float groundDeceleration = 100f;
        
        [Range(0f, 1f)] public float airborneAccelProportion;
        [Range(0f, 1f)] public float airborneDecelProportion;

        [SerializeField] public float dashSpeed; // = 10f;
        [SerializeField] public float defaultTime = 0.1f;
        [SerializeField] public float attackMove = 1f;
        [SerializeField] public float maxComboDelay;
        [SerializeField] public float hurtMove = 1f;
        
        [SerializeField] public int attack_x;
        [SerializeField] public int attack_y;
        [SerializeField] public int damage;
        [SerializeField] public int powerAttackdamage;
        
        [SerializeField] public int paring_x;
        [SerializeField] public int paring_y;
        
    }
}