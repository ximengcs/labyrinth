using UnityEngine;
using Labyrinth.Utility;
using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// 深度优先生成算法
    /// </summary>
    public class DeepSearchGenerateAlgorithm : IGenerateAlgorithm
    {
        private Map m_Map;
        private HashSet<Node> m_Nodes;

        public DeepSearchGenerateAlgorithm()
        {
            m_Nodes = new();
        }

        public void Execute(Map map)
        {
            m_Map = map;
            RecursiveAssign(m_Map.Nodes[0, 0]);
        }

        private void RecursiveAssign(Node node)
        {
            if (m_Nodes.Contains(node))
                return;
            m_Nodes.Add(node);
            foreach (int i in RandSequence())
            {
                switch (i)
                {
                    case Constant.LEFT: AssignLeft(node); break;
                    case Constant.RIGHT: AssignRight(node); break;
                    case Constant.TOP: AssignTop(node); break;
                    case Constant.BOTTOM: AssignBottom(node); break;
                }
            }
        }

        private int[] RandSequence()
        {
            int[] result = new int[] { Constant.LEFT, Constant.RIGHT, Constant.TOP, Constant.BOTTOM };
            int length = result.Length;
            for (int i = 0; i < length; i++)
            {
                int target = Random.Range(0, length) % length;
                if (target > 0)
                {
                    int tmp = result[i];
                    result[i] = result[target];
                    result[target] = tmp;
                }
            }
            return result;
        }

        private void AssignLeft(Node origin)
        {
            Node node = origin.LeftNode;
            if (node != null)
            {
                if (!m_Nodes.Contains(node))
                {
                    origin.LeftSide = Side.Pass;
                    node.RightSide = Side.Pass;
                    RecursiveAssign(node);
                }
            }
        }

        private void AssignRight(Node origin)
        {
            Node node = origin.RightNode;
            if (node != null)
            {
                if (!m_Nodes.Contains(node))
                {
                    origin.RightSide = Side.Pass;
                    node.LeftSide = Side.Pass;
                    RecursiveAssign(node);
                }
            }
        }

        private void AssignTop(Node origin)
        {
            Node node = origin.TopNode;
            if (node != null)
            {
                if (!m_Nodes.Contains(node))
                {
                    origin.TopSide = Side.Pass;
                    node.BottomSide = Side.Pass;
                    RecursiveAssign(node);
                }
            }
        }

        private void AssignBottom(Node origin)
        {
            Node node = origin.BottomNode;
            if (node != null)
            {
                if (!m_Nodes.Contains(node))
                {
                    origin.BottomSide = Side.Pass;
                    node.TopSide = Side.Pass;
                    RecursiveAssign(node);
                }
            }
        }
    }
}
