namespace Blind
{
    public class FOWTile
    {
        public TilePos pos;

        /// <summary> (x,y) 좌표, width를 이용해 계산한 일차원 배열 내 인덱스 </summary>
        public int index;

        public int X => pos.x;
        public int Y => pos.y;

        public bool isBlock = false;

        public FOWTile(bool isBlock, int x, int y, int width)
        {
            pos.x = x;
            pos.y = y;
            this.isBlock = isBlock;

            index = x + y * width;
        }
    }
}