using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Blind
{
    public class SightController : SingletonDontCreate<SightController>
    {
         public bool ShowGizmos = true;
        [Space(8f)]
        public float TileSize = 1; // 타일 하나의 크기

        private List<TilePos> _visibleTiles = new List<TilePos>();
        private List<SightUnit> _unitList = new List<SightUnit>();

        private Vector3 _size;
        private Vector3 _origin;
        private Vector3 _center;
        private Bounds _bounds;

        private FOWMap _fowMap;
        private FOWRenderer _firstLayer;

        protected override void Awake()
        {
            base.Awake();
            _firstLayer = transform.GetChild(0).GetComponent<FOWRenderer>();
        }

        private void Start()
        {
            _size = _firstLayer.Size;
            _bounds = _firstLayer.Bounds;
            _center = _bounds.center;
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
                foreach (var unit in _unitList)
                {
                    for (int i = -unit.Range; i < unit.Range; i++)
                    {
                        for (int j = -unit.Range; j < unit.Range; j++)
                        {
                            _visibleTiles.Add(GetUnitTilePos(unit.transform) + new TilePos(i, j));
                        }
                    }
                }

                var positions = new List<Vector2>();
                foreach (var tile in _visibleTiles)
                {
                    positions.Add(GetTileCenterPos(tile));
                }

                _firstLayer.DrawTexture(positions);
                yield return new WaitForSeconds(0.02f);
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
                    var ray = new Ray(GetTileCenterPos(i, j), new Vector3(0, 0, 1) * 10);
                    RaycastHit hit;
                    var raycast = Physics.Raycast(ray, out hit);
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
            var tmp = (pos.position - _origin) / TileSize;
            return new TilePos((int) tmp.x, (int) tmp.y);
        }

        private Vector2 GetTileCenterPos(TilePos tile)
        {
            return GetTileCenterPos(tile.x, tile.y);
        }

        private Vector2 GetTileCenterPos(int x, int y)
        {
            var res = new Vector2(x * TileSize + TileSize / 2, y * TileSize + TileSize / 2);
            res = (Vector2) _origin + res;
            return res;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;

            foreach (var tile in _visibleTiles)
            {
                Vector2 pos = GetTileCenterPos(tile.x, tile.y);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0f), new Vector3(TileSize, TileSize, 1f));
            }

            if (ShowGizmos == false) return;
            for (int i = 0; i < _fowMap.MapSizeX; i++)
            {
                for (int j = 0; j < _fowMap.MapSizeY; j++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(GetTileCenterPos(i, j), new Vector3(TileSize, TileSize, 0));
                }
            }
        }
    }
}
