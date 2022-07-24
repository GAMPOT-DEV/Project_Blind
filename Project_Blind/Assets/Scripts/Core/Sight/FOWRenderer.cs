using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Blind
{
    public class FOWRenderer : MonoBehaviour
    {
        public float brushSize = 0.1f;
        public float Offset = 3f;
        
        private Texture2D _brushTexture;
        private Texture2D _mainTex;
        private Texture2D _originalRT;
        private MeshRenderer _mr;
        private RenderTexture _rt;
        private RenderTexture _preRt;

        private int _resolution = 512;

        public Vector3 Size { get; private set; }
        public Bounds Bounds { get; private set; }

        private void Awake()
        {
            TryGetComponent(out _mr);
            _mr.enabled = true;
            
            // 사이즈 측정
            
            Bounds = _mr.bounds;
            Size = Bounds.size;
            
            _resolution = (int)Size.x * 64;

            if (_mr.material.mainTexture != null)
            {
                _mainTex = _mr.material.mainTexture as Texture2D;
            }
            // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
            else
            {
                _mainTex = new Texture2D(_resolution, _resolution);
            }
            
            
            _rt = new RenderTexture(_resolution, _resolution, 32);
            _preRt  = new RenderTexture(_resolution,_resolution,32);

            _originalRT = new Texture2D(_mainTex.width, _mainTex.height);
            
            Color32[] resetColorArray = _originalRT.GetPixels32();

            for (int i = 0; i < resetColorArray.Length; i++) {
                resetColorArray[i] = Color.white;
            }
      
            _originalRT.SetPixels32(resetColorArray);
            _originalRT.Apply();

            Graphics.Blit(_originalRT,_preRt);
            Graphics.Blit(_originalRT,_rt);

            _mr.material.SetTexture("_RenderTexture",_rt);

            // 시야 텍스쳐 추가
            _brushTexture = new Texture2D(_resolution, _resolution);
            int rSquared = (_resolution/2) * (_resolution/2);
            int x = _resolution / 2, y = x;
            for (int u = 0; u < _resolution; u++)
            {
                for (int v = 0; v < _resolution; v++)
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
        public void DrawTexture(IEnumerable<Vector2> positions)
        {
            var res = new List<Vector2>();
            Profiler.BeginSample("UvPoint Calc");
            foreach (var pos in positions)
            {
                Vector2 size = (Bounds.max - Bounds.min);
                Vector2 uvPoint = ((Vector3)pos - Bounds.min) / size;
                uvPoint = new Vector2(1,1)-uvPoint;
                uvPoint *= _resolution;
                res.Add(uvPoint);
            }
            Profiler.EndSample();
            _DrawTexture(res);
        }


        /// <summary> 렌더 텍스쳐에 브러시 텍스쳐로 그리기 </summary>
        private void _DrawTexture(in IEnumerable<Vector2> uvList)
        {
            var sightSetAlpha = Resources.Load<Material>("Materials/Sight/SightSetAlpha");
            sightSetAlpha.SetFloat("_Alpha",0.5f);
            Material blurMat = Resources.Load<Material>("Materials/Sight/SightBlur");
            blurMat.SetFloat("_Offset",Offset);
            
            Graphics.Blit(_preRt, _rt);
            RenderTexture.active = _rt; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
            GL.PushMatrix();                                  // 매트릭스 백업
            GL.LoadPixelMatrix(0, _resolution, _resolution, 0); // 알맞은 크기로 픽셀 매트릭스 설정

            float brushPixelSize = brushSize * _resolution;
            
            Profiler.BeginSample("_DrawTexture");
            // 렌더 텍스쳐에 브러시 텍스쳐를 이용해 그리기
            foreach (var uv in uvList)
            {
                Graphics.DrawTexture(
                    new Rect(
                        uv.x - brushPixelSize * 0.5f,
                        (_rt.height - uv.y) - brushPixelSize * 0.5f,
                        brushPixelSize,
                        brushPixelSize
                    ),
                    _brushTexture
                );
            }
            Profiler.EndSample();
            GL.PopMatrix();              // 매트릭스 복구
            RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
            
            Graphics.Blit(_rt,_preRt,sightSetAlpha,0);
            Graphics.Blit(_rt,_rt,blurMat,0);
            Graphics.Blit(_rt,_rt,blurMat,0);
        }
    }
}