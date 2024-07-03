
namespace Labyrinth.Core
{
    /// <summary>
    /// 生成算法
    /// </summary>
    public interface IGenerateAlgorithm
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="map">地图实例</param>
        void Execute(Map map);
    }
}
