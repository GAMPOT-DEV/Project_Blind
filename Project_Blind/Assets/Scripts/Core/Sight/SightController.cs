using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Blind
{
    public class SightController : Singleton<SightController>
    {
        public int resolution = 512;
        [Range(0.01f, 1f)]
        public float brushSize = 0.1f;
        public Texture2D brushTexture;
        public GameObject target;
        private Texture2D mainTex;
        private MeshRenderer mr;
        private RenderTexture rt;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out mr);
            rt = new RenderTexture(resolution, resolution, 32);

            if (mr.material.mainTexture != null)
            {
                mainTex = mr.material.mainTexture as Texture2D;
            }
            // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
            else
            {
                mainTex = new Texture2D(resolution, resolution);
            }

            // 메인 텍스쳐 -> 렌더 텍스쳐 복제
            Graphics.Blit(mainTex, rt);

            // 렌더 텍스쳐를 메인 텍스쳐에 등록
            mr.material.mainTexture = rt;

            // 브러시 텍스쳐가 없을 경우 임시 생성(red 색상)
            if (brushTexture == null)
            {
                brushTexture = new Texture2D(resolution, resolution);
                for (int i = 0; i < resolution; i++)
                    for (int j = 0; j < resolution; j++)
                        brushTexture.SetPixel(i, j, new Color(0,0,0,0.5f));
                brushTexture.Apply();
            }
        }

        public void DrawTexture(RaycastHit hit)
        {
            var col = hit.collider;
            if (col && col.transform == transform)
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV *= resolution;
                Debug.Log(pixelUV);
                _DrawTexture(pixelUV);
            }
        }


        /// <summary> 렌더 텍스쳐에 브러시 텍스쳐로 그리기 </summary>
        private void _DrawTexture(in Vector2 uv)
        {
            Debug.Log(uv);
            RenderTexture.active = rt; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
            GL.PushMatrix();                                  // 매트릭스 백업
            GL.LoadPixelMatrix(0, resolution, resolution, 0); // 알맞은 크기로 픽셀 매트릭스 설정

            float brushPixelSize = brushSize * resolution;
            
            // 렌더 텍스쳐에 브러시 텍스쳐를 이용해 그리기
            Graphics.DrawTexture(
                new Rect(
                    uv.x - brushPixelSize * 0.5f,
                    (rt.height - uv.y) - brushPixelSize * 0.5f,
                    brushPixelSize,
                    brushPixelSize
                ),
                brushTexture
            );

            GL.PopMatrix();              // 매트릭스 복구
            RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
        }
    }
}
