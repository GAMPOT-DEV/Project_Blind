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
        public Material FogMaterial;
        public Transform FogCam;
        [Range(1f, 20f)]
        public float SightRange = 5f;
        
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Mesh _mesh;

        private MeshData _meshData;
        
        private Color[] _colorArray;
        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            _meshData = new MeshData();
            
            
            GenerateMeshPlane(_meshData);
            _mesh = new Mesh();
            _filter.mesh = _mesh;
            _mesh.vertices = _meshData.Verts;
            _mesh.triangles = _meshData.Tris;
            
            InitBlackColor();
        }
        
        private void InitBlackColor()
        {
            Color[] colors = new Color[_mesh.vertexCount];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(1,0,0);
            }

            _mesh.colors = colors;
        }

        
        private void GenerateMeshPlane(MeshData meshData)
        {
            Vector3 widthV3 = new Vector3(Width, 0f, Width);
            Vector3 startPoint = -widthV3 * 0.5f;
            meshData.GridUnit = Vector2.one*(Width / Resolution);

            meshData.VCount = new Vector2Int(Resolution + 1, Resolution + 1);
            int vertsCount = meshData.VCount.x * meshData.VCount.y;
            int trisCount = Resolution * Resolution * 6;

            meshData.Verts = new Vector3[vertsCount];
            meshData.Tris = new int[trisCount];

            for (int j = 0; j < meshData.VCount.y; j++)
            {
                for (int i = 0; i < meshData.VCount.x; i++)
                {
                    int index = i + j * meshData.VCount.x;
                    meshData.Verts[index] = startPoint
                                            + new Vector3(
                                                meshData.GridUnit.x * i,
                                                0f,
                                                meshData.GridUnit.y * j
                                            );
                }
            }

            int tIndex = 0;
            for (int j = 0; j < meshData.VCount.y - 1; j++)
            {
                for (int i = 0; i < meshData.VCount.x - 1; i++)
                {
                    int vIndex = i + j * meshData.VCount.x;
                    
                    var tris = meshData.Tris;
                    
                    tris[tIndex + 0] = vIndex;
                    tris[tIndex + 1] = vIndex + meshData.VCount.x;
                    tris[tIndex + 2] = vIndex + 1;

                    tris[tIndex + 3] = vIndex + meshData.VCount.x;
                    tris[tIndex + 4] = vIndex + meshData.VCount.x + 1;
                    tris[tIndex + 5] = vIndex + 1;

                    tIndex += 6;
                }
            }
        }
    }
}
