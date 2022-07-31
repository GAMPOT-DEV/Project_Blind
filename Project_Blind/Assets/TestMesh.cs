using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Unity에서는 opengl처럼 GL_TRIANGLE_STRIP, GL_TRIANGLE_FAN을 지원하지 않는듯함.
// 오직. GL_TRIANGLES 삼각형 조합만 지원하는갑다.

public class TestMesh : MonoBehaviour
{
    readonly Matrix4x4 m_LookMatrix = Matrix4x4.TRS(new Vector3(0, 0, -1), Quaternion.identity, Vector3.one);
    readonly Matrix4x4 m_OrthoMatrix = Matrix4x4.Ortho(-1, 1, -1, 1, 0.01f, 2);
    Matrix4x4 m_TransformTRS;
    Texture2D m_Texture2D;
    RenderTexture m_RenderTexture;
    CommandBuffer m_CommandBuffer;
    private MeshRenderer m_TestDisplay;
    private int m_MeshScale = 3;
    public Mesh mesh;
    public Material material;

    private Vector2 m_Resolution = new Vector2(512, 512);
    void Start()
    {
        m_TestDisplay = GetComponent<MeshRenderer>();
        m_RenderTexture = new RenderTexture((int) m_Resolution.x, (int) m_Resolution.y, 24);
        m_TestDisplay.material.mainTexture = m_RenderTexture;
        m_CommandBuffer = new CommandBuffer();
        m_CommandBuffer.name = "SplineBaker";
        m_TransformTRS = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * m_MeshScale);
    }
 
    void Update()
    {
        m_CommandBuffer.Clear();
        m_CommandBuffer.SetRenderTarget(m_RenderTexture);
        m_CommandBuffer.ClearRenderTarget(true, true, Color.clear);
        m_CommandBuffer.SetViewProjectionMatrices(m_LookMatrix, m_OrthoMatrix);
        m_CommandBuffer.DrawMesh(mesh, m_TransformTRS, material, 0);
        Graphics.ExecuteCommandBuffer(m_CommandBuffer);
    }
}
