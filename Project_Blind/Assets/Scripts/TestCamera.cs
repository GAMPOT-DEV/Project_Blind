using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeastCamera : MonoBehaviour
{

    public Vector3 offset;

    public float followSpeed;

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 camera_pos = player.transform.position + offset;
        Vector3 lerp_pos = Vector3.Lerp(transform.position, camera_pos, followSpeed);
        transform.position = lerp_pos;
        transform.LookAt(player.transform);
    }
}
