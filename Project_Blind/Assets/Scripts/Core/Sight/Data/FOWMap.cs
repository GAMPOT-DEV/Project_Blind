using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class FOWMap
    {
        private List<FOWTile> map = new List<FOWTile>();

        public int MapSizeX { get; private set; }
        public int MapSizeY { get; private set; }

        public Vector2 MapSize
        {
            get
            {
                return new Vector3(MapSizeX,MapSizeY);
            }
        }

        public FOWMap(int x,int y)
        {
            MapSizeX = x;
            MapSizeY = y;
        }
        
    }
}