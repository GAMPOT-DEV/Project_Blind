namespace Blind
{
    public class FOWTile
    {
        public TilePos pos;

        /// <summary> (x,y) 좌표, width를 이용해 계산한 일차원 배열 내 인덱스 </summary>
        public int Index;

        public int X => pos.x;
        public int Y => pos.y;

        public bool IsBlock = false;

        public FOWTile(bool isBlock, int x, int y, int width)
        {
            pos.x = x;
            pos.y = y;
            IsBlock = isBlock;

            Index = x + y * width;
        }
    }
}