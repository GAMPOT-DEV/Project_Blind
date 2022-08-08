using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool attackable;
    private float _avoidRange = 9f;

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
            attackable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            attackable = false;
    }
}
