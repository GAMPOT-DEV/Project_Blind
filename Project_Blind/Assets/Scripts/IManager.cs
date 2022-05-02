namespace Blind
{
    /// <summary>
    /// 매니저의 인터페이스입니다. 매니저들 싱글턴이 구현되어 있습니다.
    /// </summary>
    public interface IManager
    {
        public static IManager Instance { get; set; }
        private static IManager Create()
        {
            throw new System.NotImplementedException();
        }
    }
}