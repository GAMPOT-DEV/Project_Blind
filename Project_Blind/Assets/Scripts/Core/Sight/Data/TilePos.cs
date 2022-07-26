namespace Blind
{
    public struct TilePos
    {
        public int x;
        public int y;

        public TilePos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int Distance(TilePos other)
        {
            int distX = other.x - x;
            int distY = other.y - x;
            return (distX * distX) + (distY * distY);
        }

        /// <summary> (x, y) 인덱스를 일차원배열의 인덱스로 변환 </summary>
        public int GetTileIndex(in int mapWidth)
        {
            return x + y * mapWidth;
        }

        public override string ToString()
        {
            return $"{x} , {y}";
        }

        public static TilePos operator +(TilePos p1,TilePos p2)
        {
            return new TilePos(p1.x + p2.x, p1.y + p2.y);
        }
        
    }
}