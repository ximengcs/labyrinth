using UnityEngine;

namespace Labyrinth.Core
{
    /// <summary>
    /// 迷宫地图节点
    /// </summary>
    public class Node
    {
        private Map m_Map;
        private Vector2Int m_Pos;

        /// <summary>
        /// 位置点
        /// </summary>
        public Vector2Int Pos => m_Pos;

        /// <summary>
        /// 所属地图
        /// </summary>
        public Map Map => m_Map;

        /// <summary>
        /// 左侧边状态
        /// </summary>
        public Side LeftSide { get; set; }

        /// <summary>
        /// 右侧边状态
        /// </summary>
        public Side RightSide { get; set; }

        /// <summary>
        /// 顶边状态
        /// </summary>
        public Side TopSide { get; set; }

        /// <summary>
        /// 底边状态
        /// </summary>
        public Side BottomSide { get; set; }

        /// <summary>
        /// 左侧节点
        /// </summary>
        public Node LeftNode => Pos.x > 0 ? m_Map.Nodes[Pos.x - 1, Pos.y] : null;

        /// <summary>
        /// 右侧节点
        /// </summary>
        public Node RightNode => Pos.x < m_Map.Width - 1 ? m_Map.Nodes[Pos.x + 1, Pos.y] : null;

        /// <summary>
        /// 顶测节点
        /// </summary>
        public Node TopNode => Pos.y < m_Map.Height - 1 ? m_Map.Nodes[Pos.x, Pos.y + 1] : null;

        /// <summary>
        /// 底测节点
        /// </summary>
        public Node BottomNode => Pos.y > 0 ? m_Map.Nodes[Pos.x, Pos.y - 1] : null;

        /// <summary>
        /// 节点是否处于地图边缘
        /// </summary>
        public bool IsSide
        {
            get
            {
                if (Pos.x == 0 || Pos.x == m_Map.Width - 1)
                    return true;
                if (Pos.y == 0 || Pos.y == m_Map.Height - 1)
                    return true;
                return false;
            }
        }

        #region 接口
        /// <summary>
        /// 节点中是否包含终点边
        /// </summary>
        /// <returns>true 包含</returns>
        public bool IsDestination()
        {
            return LeftSide == Side.Destination ||
                RightSide == Side.Destination ||
                TopSide == Side.Destination ||
                BottomSide == Side.Destination;
        }

        /// <summary>
        /// 设置此节点为终点
        /// </summary>
        public void SetAsDestination()
        {
            if (Pos.x == 0)
                LeftSide = Side.Destination;
            else if (Pos.x == m_Map.Width - 1)
                RightSide = Side.Destination;
            else if (Pos.y == 0)
                BottomSide = Side.Destination;
            else if (Pos.y ==  m_Map.Height - 1)
                TopSide = Side.Destination;
        }
        #endregion

        #region 生命周期
        public void OnInit(Map map, Vector2Int pos)
        {
            m_Pos = pos;
            m_Map = map;
        }

        public void OnDispose()
        {
            m_Map = null;
        }
        #endregion
    }
}
