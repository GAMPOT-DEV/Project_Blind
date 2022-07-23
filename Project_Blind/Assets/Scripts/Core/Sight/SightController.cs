using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Blind
{
    public class SightController : SingletonDontCreate<SightController>
    {
        public int resolution = 512;
        [Range(0.01f, 1f)]
        public float brushSize = 0.1f;
        public bool ShowGizmos = true;
        
        public const float CurrentAlpha = 0f; // 현재 위치한 경우 알파값
        public const float NeverAlpha = 1f; // 방문한 적 없는 경우 알파값

        public LayerMask _groundLayer;

        [Space(8f)]
        public float TileSize = 1; // 타일 하나의 크기
        
        public float Offset = 3f; // 타일 하나의 크기

        
        private Texture2D _brushTexture;
        private Texture2D _mainTex;
        private Texture2D _originalRT;
        private MeshRenderer _mr;
        private RenderTexture _rt;
        private RenderTexture _rtBuffer;
        
        private Vector3 _size;
        private Vector3 _center;
        private Vector3 _origin;
        private Bounds _bounds;
        
        private List<TilePos> _visibleTiles = new List<TilePos>();
        private List<SightUnit> _unitList = new List<SightUnit>();

        private FOWMap _fowMap;

        protected override void Awake()
        {
            base.Awake();
            GetComponent<MeshRenderer>().enabled = true;
            TryGetComponent(out _mr);

            _rt = new RenderTexture(resolution, resolution, 32);
            _rtBuffer  = new RenderTexture(resolution,resolution,32);

            if (_mr.material.mainTexture != null)
            {
                _mainTex = _mr.material.mainTexture as Texture2D;
            }
            // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
            else
            {
                _mainTex = new Texture2D(resolution, resolution);
            }

            _bounds = _mr.localBounds;
            _originalRT = new Texture2D(_mainTex.width, _mainTex.height);
            
            Color32[] resetColorArray = _originalRT.GetPixels32();

            for (int i = 0; i < resetColorArray.Length; i++) {
                resetColorArray[i] = Color.white;
            }
      
            _originalRT.SetPixels32(resetColorArray);
            _originalRT.Apply();

            Graphics.Blit(_originalRT,_rt);

            _mr.material.SetTexture("_RenderTexture",_rt);

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
            
            // size측정
            bounds = _mr.bounds;
            _size = bounds.size;
            _center = bounds.center;
            int sizeX = (int)(_size.x / (float)TileSize);
            int sizeY = (int)(_size.y / (float)TileSize);
            _fowMap = new FOWMap(sizeX,sizeY);
            _origin = _center - (Vector3)_fowMap.MapSize / 2;

            FowMapInit();
            StartCoroutine(UpdateFog());
        }

        private IEnumerator UpdateFog()
        {
            while (true)
            {
                _visibleTiles.Clear();
                foreach(var unit in _unitList)
                {
                    for (int i = -unit.Range; i < unit.Range; i++)
                    {
                        for (int j = -unit.Range; j < unit.Range; j++)
                        {
                            _visibleTiles.Add(GetUnitTilePos(unit.transform) + new TilePos(i,j));
                        }
                    }
                }
                var positions = new List<Vector2>();
                foreach (var tile in _visibleTiles)
                {
                    positions.Add(GetTileCenterPos(tile));
                }
                DrawTexture(positions); yield return new WaitForSeconds(0.02f); 
            }
        }
        
        /// <summary>
        /// 맵 초기화
        /// </summary>
        public void FowMapInit()
        {
            var mapSize = _fowMap.MapSize;
            for (var i = -1; i < mapSize.x; i++)
            {
                for (var j = -1; j < mapSize.y; j++)
                {
                    int layermask = 1 << LayerMask.NameToLayer("Floor");
                    var ray = new Ray(GetTileCenterPos(i, j), new Vector3(0,0,1) * 10);
                    RaycastHit hit;
                    var raycast = Physics.Raycast(ray,out hit);
                    if (raycast)
                    {
                        Debug.Log(10);
                    }
                }
            }
        }

        public void AssignUnit(SightUnit unit)
        {
            _unitList.Add(unit);
        }

        private TilePos GetUnitTilePos(Transform pos)
        {
            var tmp = (pos.position - _origin)/TileSize;
            return new TilePos((int)tmp.x, (int)tmp.y);
        }

        private Vector2 GetTileCenterPos(TilePos tile)
        {
            return GetTileCenterPos(tile.x, tile.y);
        }

        private Vector2 GetTileCenterPos(int x,int y)
        {
            var res = new Vector2(x * TileSize + TileSize/2 , y * TileSize + TileSize / 2);
            res = (Vector2)_origin + res;
            return res;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;
            
            foreach (var tile in _visibleTiles)
            {
                Vector2 pos = GetTileCenterPos(tile.x, tile.y);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(pos.x,pos.y,0f), new Vector3(TileSize, TileSize,1f));
            }
            
            if (ShowGizmos == false) return;
            for (int i = 0; i < _fowMap.MapSizeX; i++)
            {
                for (int j = 0; j < _fowMap.MapSizeY; j++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(GetTileCenterPos(i,j),new Vector3(TileSize,TileSize,0));
                }
            }
        }

        public void DrawTexture(IEnumerable<Vector2> positions)
        {
            Profiler.BeginSample("DrawTexture");
            var res = new List<Vector2>();
            foreach (var pos in positions)
            {
                var ray = new Ray((Vector3)pos + new Vector3(0,0,-1),new Vector3(0,0,1) * 10 );
                RaycastHit hit;
                var raycast = Physics.Raycast(ray,out hit);
                var col = hit.collider;
                if (!raycast || !col || col.transform != transform) return;
                
                var pixelUV = hit.textureCoord;
                pixelUV *= resolution;
                res.Add(pixelUV);
            }

            _DrawTexture(res);
            Profiler.EndSample();
        }


        /// <summary> 렌더 텍스쳐에 브러시 텍스쳐로 그리기 </summary>
        private void _DrawTexture(in IEnumerable<Vector2> uvList)
        {   
            Profiler.BeginSample("_DrawTexture");
            Graphics.Blit(_originalRT, _rtBuffer);
            RenderTexture.active = _rtBuffer; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
            GL.PushMatrix();                                  // 매트릭스 백업
            GL.LoadPixelMatrix(0, resolution, resolution, 0); // 알맞은 크기로 픽셀 매트릭스 설정

            float brushPixelSize = brushSize * resolution;
            
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
            GL.PopMatrix();              // 매트릭스 복구
            RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
            Profiler.EndSample();

            Material blurMat = Resources.Load<Material>("Materials/SightBlur");
            blurMat.SetFloat("_Offset",Offset);
            Graphics.Blit(_rtBuffer,_rt,blurMat,0);
            Graphics.Blit(_rt,_rtBuffer,blurMat,0);
            Graphics.Blit(_rtBuffer,_rt,blurMat,0);
        }
    }
}
