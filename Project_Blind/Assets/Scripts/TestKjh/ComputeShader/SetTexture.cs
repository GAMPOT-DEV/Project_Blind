using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTexture : MonoBehaviour
{
    private ComputeShaderTest _dispatch;
    private MeshRenderer mr;
    private int prop_MainTex;

    private void Awake()
    {
        _dispatch = this.GetComponent<ComputeShaderTest>();
        mr = this.GetComponent<MeshRenderer>();
        prop_MainTex = Shader.PropertyToID("_MainTex");
    }
    private void Update()
    {
        mr.sharedMaterial.SetTexture(prop_MainTex, _dispatch._texture);
    }
}
