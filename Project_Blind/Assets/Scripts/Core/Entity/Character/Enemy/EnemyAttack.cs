using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool attackable;
    private float _avoidDis = 9f;
    private Transform player;

    public void setRange(Vector2 range)
    {
        gameObject.GetComponent<BoxCollider2D>().size = range;
    }

    public bool Attackable()
    {
        return attackable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attackable = true;
            if (player == null)
                player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            attackable = false;
    }

    public bool isAvoid()
    {
        if(attackable)
        {
            float distance = player.position.x - GetComponentInParent<Transform>().position.x;
            if (distance < _avoidDis)
            {
                return true;
            }
        }
        return false;
    }
}
