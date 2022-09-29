using UnityEngine;

namespace Blind.ScriptableObjects
{
    [ CreateAssetMenu( fileName = "Enemy", menuName = "ScriptableObject/Enemy" )]
    public class EnemyCharacter : Character
    {
        [SerializeField] public Vector2 sensingRange;
        [SerializeField] public float speed;
        [SerializeField] public float runSpeed;
        [SerializeField] public float attackCoolTime;
        [SerializeField] public float attackSpeed;
        [SerializeField] public int damage;
        [SerializeField] public float stunTime;
        [SerializeField] public float HpBarHeight;

        //Crowd Only
        [SerializeField] public float patrolTime;
        [SerializeField] public Vector2 BasicAttackRange;
        [SerializeField] public Vector2 SkillAttackRange;
    }
}