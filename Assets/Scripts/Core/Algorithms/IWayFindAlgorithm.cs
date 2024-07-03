
namespace Labyrinth.Core
{
    /// <summary>
    /// 寻路算法
    /// </summary>
    public interface IWayFindAlgorithm
    {
        /// <summary>
        /// 执行算法
        /// </summary>
        /// <param name="node">起始点</param>
        /// <returns>最短出口路径</returns>
        Path Execute(Node node);
    }
}
