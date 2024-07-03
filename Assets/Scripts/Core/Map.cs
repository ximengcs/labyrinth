using System;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth.Core
{
    /// <summary>
    /// 迷宫地图
    /// </summary>
    public class Map
    {
        /// <summary>
        /// 出口数量
        /// </summary>
        public const int EXIT_COUNT = 3;

        private int m_Id;
        private int m_Width;
        private int m_Height;
        private Node[,] m_Nodes;

        /// <summary>
        /// 实例Id
        /// </summary>
        public int Id => m_Id;

        /// <summary>
        /// 宽
        /// </summary>
        public int Width => m_Width;

        /// <summary>
        /// 高
        /// </summary>
        public int Height => m_Height;

        /// <summary>
        /// 所有节点
        /// </summary>
        public Node[,] Nodes => m_Nodes;

        #region 相关事件
        public event Action InitEvent;
        public event Action DisposeEvent;
        public event Action<long> RefreshEvent;
        #endregion

        #region 生命周期
        public void OnInit(int id, int width, int height)
        {
            m_Id = id;
            m_Width = width;
            m_Height = height;
            m_Nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Node node = new Node();
                    node.OnInit(this, new Vector2Int(x, y));
                    m_Nodes[x, y] = node;
                }
            }
            InitEvent?.Invoke();
        }

        public void OnDispose()
        {
            foreach (Node node in m_Nodes)
                node.OnDispose();
            m_Nodes = null;
            DisposeEvent?.Invoke();
        }
        #endregion

        #region 接口
        /// <summary>
        /// 重置地图
        /// </summary>
        public void Reset()
        {
            foreach (Node node in m_Nodes)
            {
                node.LeftSide = Side.Block;
                node.RightSide = Side.Block;
                node.TopSide = Side.Block;
                node.BottomSide = Side.Block;
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            IGenerateAlgorithm algorithm = new DeepSearchGenerateAlgorithm();
            algorithm.Execute(this);
            sw.Stop();

            List<Node> sideNodes = new List<Node>();
            foreach (Node node in m_Nodes)
            {
                if (node.IsSide)
                    sideNodes.Add(node);
            }

            for (int i = 0; i < EXIT_COUNT; i++)
            {
                int rand = UnityEngine.Random.Range(0, sideNodes.Count);
                Node node = sideNodes[rand];
                if (!node.IsDestination())
                    node.SetAsDestination();
                else
                    i--;
            }

            RefreshEvent?.Invoke(sw.ElapsedMilliseconds);
        }
        #endregion
    }
}
