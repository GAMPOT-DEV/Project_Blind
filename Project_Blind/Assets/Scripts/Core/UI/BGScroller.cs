using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private float[] offsets = new float[4];
    [SerializeField]
    private float[] speeds = new float[4];
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            offsets[i] += Time.deltaTime * speeds[i];
            if (i == 1)
            {
                _meshRenderer.materials[i].mainTextureOffset = new Vector2(0, offsets[i]);
            }
            else
            {
                _meshRenderer.materials[i].mainTextureOffset = new Vector2(offsets[i], 0);
            }
            
        }
    }
}
