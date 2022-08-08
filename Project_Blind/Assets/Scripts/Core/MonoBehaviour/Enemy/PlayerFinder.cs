using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject edge;
    private Transform player;
    private float avoidDis = 5.5f;
    private float chaseDis = 10.5f;

    public void setRange(Vector2 range)
    {
        gameObject.GetComponent<BoxCollider2D>().size = range;
    }

    public Vector2 ChasePlayer()
    {
        Vector2 position = GetComponentInParent<Transform>().position;
        Vector2 direction = new Vector2(player.position.x - position.x, 0);
        direction.x /= Mathf.Abs(direction.x);
        //direction.y /= Mathf.Abs(direction.y);

        return direction;
    }

    public Transform PlayerPosition()
    {
        return player;
    }

    public bool FindPlayer()
    {
        return playerInRange;
    }

    public bool MissPlayer()
    {
        return !playerInRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
            edge = collision.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (edge == null || Nearer(collision.transform))
            {
                playerInRange = true;
                player = collision.transform;
            }
            else
                playerInRange = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
        }
        if (collision.gameObject.layer == 7)
            edge = null;
    }

    private bool Nearer(Transform player)
    {
        Vector2 position = GetComponentInParent<Transform>().position;
        float playerDis = Vector2.Distance(player.position, position);
        float edgeDis = Vector2.Distance(edge.transform.position, position);
        if (playerDis >= edgeDis)
            return false;
        else
            return true;
    }

    public int AvoidOrChase()
    {
        Vector2 position = GetComponentInParent<Transform>().position;
        //if (Mathf.Abs(player.position.x - position.x) < avoidDis)
        if (Vector2.Distance(player.position, position) < avoidDis)
            return 1;
        else if (Mathf.Abs(player.position.x - position.x) > chaseDis)
            return -1;
        else return 0;
    }
}
