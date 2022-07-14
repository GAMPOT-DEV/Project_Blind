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
        public bool ShowGizmos = true;
        
        public const float CurrentAlpha = 0f; // 현재 위치한 경우 알파값
        public const float NeverAlpha = 1f; // 방문한 적 없는 경우 알파값

        public LayerMask _groundLayer;

        [Space(8f)]
        public float FogWidthX = 20;
        public float FogWidthZ = 20;
        public float TileSize = 1; // 타일 하나의 크기

        
        private Texture2D _brushTexture;
        private Texture2D _mainTex;
        private MeshRenderer _mr;
        private RenderTexture _rt;
        private RenderTexture _originalRT;

        protected override void Awake()
        {
            base.Awake();
            GetComponent<MeshRenderer>().enabled = true;
            TryGetComponent(out _mr);
            _rt = new RenderTexture(resolution, resolution, 32);

            if (_mr.material.mainTexture != null)
            {
                _mainTex = _mr.material.mainTexture as Texture2D;
            }
            // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
            else
            {
                _mainTex = new Texture2D(resolution, resolution);
            }

            // 메인 텍스쳐 -> 렌더 텍스쳐 복제
            Graphics.Blit(_mainTex, _rt);

            // 렌더 텍스쳐를 메인 텍스쳐에 등록
            _mr.material.mainTexture = _rt;

            // 시야 텍스쳐 추가
            _brushTexture = new Texture2D(resolution, resolution);
            int rSquared = (resolution/2) * (resolution/2);
            int x = resolution / 2, y = x;
            for (int u = 0; u < resolution; u++)
            {
                for (int v = 0; v < resolution; v++)
                {
                    if ((x-u)*(x-u) + (y-v)*(y-v) < rSquared) {
                        _brushTexture.SetPixel(u, v, new Color(0,0,0,1f));
                    }
                    else
                    {
                        _brushTexture.SetPixel(u, v, new Color(0,0,0,0));
                    }
                }
            }
            _brushTexture.Apply();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;
#if DEBUG_RANGE
            foreach (var tile in visibleTiles)
            {
                Vector2 pos = GetTileCenterPoint(tile.x, tile.y);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(pos.x, 0f, pos.y), new Vector3(_tileSize, 1f, _tileSize));
            }
#endif
            if (ShowGizmos == false) return;
            
        }

        public void DrawTexture(RaycastHit hit)
        {
            var col = hit.collider;
            if (col && col.transform == transform)
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV *= resolution;
                _DrawTexture(pixelUV);
            }
        }


        /// <summary> 렌더 텍스쳐에 브러시 텍스쳐로 그리기 </summary>
        private void _DrawTexture(in Vector2 uv)
        {
            Graphics.Blit(_mainTex, _rt);
            RenderTexture.active = _rt; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
            GL.PushMatrix();                                  // 매트릭스 백업
            GL.LoadPixelMatrix(0, resolution, resolution, 0); // 알맞은 크기로 픽셀 매트릭스 설정

            float brushPixelSize = brushSize * resolution;
            
            // 렌더 텍스쳐에 브러시 텍스쳐를 이용해 그리기
            Graphics.DrawTexture(
                new Rect(
                    uv.x - brushPixelSize * 0.5f,
                    (_rt.height - uv.y) - brushPixelSize * 0.5f,
                    brushPixelSize,
                    brushPixelSize
                ),
                _brushTexture
            );
            
            GL.PopMatrix();              // 매트릭스 복구
            RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
        }
    }
}
