using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{

    public Vector3 offset;

    public float followSpeed = 3f;

    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player(animation)");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 camera_pos = player.transform.position - this.transform.position;
        Vector3 lerp_pos = new Vector3(camera_pos.x * followSpeed * Time.deltaTime,
            (camera_pos.y + 6f) * followSpeed * Time.deltaTime, 0f);
        this.transform.Translate(lerp_pos);
    }
}
