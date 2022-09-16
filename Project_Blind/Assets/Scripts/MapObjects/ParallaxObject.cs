using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField] private float multiplier = 0.0f;
    [SerializeField] private bool horizontalOnly = true;
    private Transform _cameraTransform;
    private Vector3 _startCameraPos;
    private Vector3 _startPos;

    void Start()
    {
        _cameraTransform = Camera.main!.transform;
        _startCameraPos = _cameraTransform.position;
        _startPos = transform.position;
    }


    private void LateUpdate()
    {
        var position = _startPos;
        if (horizontalOnly)
            position.x += multiplier * (_cameraTransform.position.x - _startCameraPos.x);
        else
            position += multiplier * (_cameraTransform.position - _startCameraPos);

        transform.position = position;
    }
}
