using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private bool playerInRange = false;
    private GameObject edge;

    public void setRange(Vector2 range)
    {
        gameObject.GetComponent<BoxCollider2D>().size = range;
    }

    public bool FindPlayer()
    {
        return playerInRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            edge = collision.gameObject;
            Debug.Log("Edge In");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (edge == null)
                playerInRange = true;
            else if(Nearer(collision.transform))
            {
                playerInRange = true;
            }
            else
                playerInRange = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerInRange = false;
        if (collision.gameObject.layer == 7)
        {
            edge = null;
            Debug.Log("Edge Out");
        }
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
}
