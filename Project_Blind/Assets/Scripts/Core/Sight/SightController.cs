using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class SightController : MonoBehaviour
    {
        public class MeshData
        {
            public Vector3[] Verts;
            public int[] Tris;
            public Vector2 GridUnit; // 한 그리드의 x, z 길이
            public Vector2Int VCount;  // 각각 x, z의 버텍스 개수
        }
        
        public int Resolution = 64;
        public float Width = 5f;
        public Transform FogCam;
        [Range(1f, 20f)]
        public float SightRange = 5f;
        
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Mesh _mesh;
        private Material _fogMaterial;

        private MeshData _meshData;
        
        private Color[] _colorArray;
        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();

            _fogMaterial = _renderer.material;
            Texture2D texbuffer = new Texture2D(500,500,TextureFormat.ARGB32,false);
            var renderBuffer = RenderTexture.GetTemporary(500, 500,0);
            
            Graphics.Blit(texbuffer,renderBuffer,_fogMaterial);
        }
        
    }
}
