
using System.Collections;
using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// 寻路路径
    /// </summary>
    public class Path : IEnumerable<Node>
    {
        private Node m_StartNode;
        private List<Node> m_Nodes;

        /// <summary>
        /// 起始节点
        /// </summary>
        public Node StartNode => m_StartNode;

        /// <summary>
        /// 到达终点所需节点数
        /// </summary>
        public int Step => m_Nodes.Count;

        public Path(Node startNode)
        {
            m_StartNode = startNode;
            m_Nodes = new List<Node>();
        }

        #region 接口
        /// <summary>
        /// 添加下一个路径点
        /// </summary>
        /// <param name="node">路径点</param>
        public void Add(Node node)
        {
            m_Nodes.Add(node);
        }

        /// <summary>
        /// 移除最后一个路径点
        /// </summary>
        public void RemoveLast()
        {
            m_Nodes.RemoveAt(m_Nodes.Count - 1);
        }

        /// <summary>
        /// 检查是否包含路径点
        /// </summary>
        /// <param name="node">路径点</param>
        /// <returns>true 包含</returns>
        public bool Contains(Node node)
        {
            return m_Nodes.Contains(node);
        }

        /// <summary>
        /// 复制此路径
        /// </summary>
        /// <returns>新路径实例</returns>
        public Path Clone()
        {
            Path path = new Path(m_StartNode);
            foreach (Node node in m_Nodes)
            {
                path.Add(node);
            }
            return path;
        }
        #endregion

        #region 迭代器
        public IEnumerator<Node> GetEnumerator()
        {
            return m_Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Nodes.GetEnumerator();
        }
        #endregion
    }
}
