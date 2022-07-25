using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{
    [SerializeField] private ComputeShader _computeShader;
    //[SerializeField] private Texture2D _texture;
    public RenderTexture _texture;
    private int KernerID;

    private struct CSPARAM
    {
        public const string KERNEL = "CSMain";
        public const string RESULT = "Result";
        public const int THREAD_NUMBER_X = 8;
        public const int THREAD_NUMBER_Y = 8;
        public const int THREAD_NUMBER_Z = 1;
    }
    void Awake()
    {
        KernerID = _computeShader.FindKernel(CSPARAM.KERNEL);
        _texture = new RenderTexture(1024, 1024, 24);
        _texture.enableRandomWrite = true;
        _texture.Create();
        Debug.Log(_texture.width);
    }
    void Update()
    {
        DispatchComputeShader();
    }
    void DispatchComputeShader()
    {
        _computeShader.SetTexture(KernerID, CSPARAM.RESULT, _texture);
        _computeShader.Dispatch(KernerID, _texture.width / CSPARAM.THREAD_NUMBER_X,
            _texture.width / CSPARAM.THREAD_NUMBER_Y, CSPARAM.THREAD_NUMBER_Z);
    }
}
