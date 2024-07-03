using UnityEngine;
using System.Collections.Generic;
using Labyrinth.Utility;

namespace Labyrinth.Core
{
    /// <summary>
    /// Kruskal算法
    /// </summary>
    public class KruskalGenerateAlgorithm : IGenerateAlgorithm
    {
        private class NodeInfo
        {
            public NodeInfo Root { get; private set; }

            private Node m_Node;
            private List<NodeInfo> m_Nodes;

            public Node Node => m_Node;

            public NodeInfo(Node node)
            {
                m_Node = node;
                m_Nodes = new();
                Root = this;
            }

            public void Add(NodeInfo node)
            {
                m_Nodes.Add(node);
                RecursiveSetRoot(node, Root);
            }

            private void RecursiveSetRoot(NodeInfo node, NodeInfo root)
            {
                if (node == null)
                    return;
                if (node.Root == root)
                    return;
                node.Root = root;
                foreach (NodeInfo child in node.m_Nodes)
                    RecursiveSetRoot(child, root);
            }
        }

        private Dictionary<Node, NodeInfo> m_AllNodes;
        private List<NodeInfo> m_Nodes;

        public void Execute(Map map)
        {
            m_AllNodes = new(map.Width * map.Height);
            foreach (Node node in map.Nodes)
                m_AllNodes.Add(node, new NodeInfo(node));

            m_Nodes = new List<NodeInfo>(m_AllNodes.Values);

            while (Step())
                ;
        }

        public bool Step()
        {
            if (m_Nodes.Count <= 1)
                return false;
            NodeInfo nodeInfo = m_Nodes[Random.Range(0, m_Nodes.Count)];
            Node targetNode = RandNode(nodeInfo);
            if (targetNode != null)
            {
                NodeInfo target = m_AllNodes[targetNode];

                nodeInfo.Add(target);
                m_Nodes.Remove(target);

                if (nodeInfo.Node.LeftNode == targetNode)
                {
                    nodeInfo.Node.LeftSide = Side.Pass;
                    targetNode.RightSide = Side.Pass;
                }
                if (nodeInfo.Node.RightNode == targetNode)
                {
                    nodeInfo.Node.RightSide = Side.Pass;
                    targetNode.LeftSide = Side.Pass;
                }
                if (nodeInfo.Node.TopNode == targetNode)
                {
                    nodeInfo.Node.TopSide = Side.Pass;
                    targetNode.BottomSide = Side.Pass;
                }
                if (nodeInfo.Node.BottomNode == targetNode)
                {
                    nodeInfo.Node.BottomSide = Side.Pass;
                    targetNode.TopSide = Side.Pass;
                }
            }
            else
            {
                m_Nodes.Remove(nodeInfo);
            }

            return true;
        }

        private Node RandNode(NodeInfo nodeInfo)
        {
            Node node = nodeInfo.Node;
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

            Node targetNode = null;
            foreach (int i in result)
            {
                switch (i)
                {
                    case Constant.LEFT:
                        targetNode = node.LeftNode;
                        if (targetNode != null && m_AllNodes[targetNode].Root != nodeInfo.Root)
                            return targetNode;
                        break;
                    case Constant.RIGHT:
                        targetNode = node.RightNode;
                        if (targetNode != null && m_AllNodes[targetNode].Root != nodeInfo.Root)
                            return targetNode;
                        break;
                    case Constant.TOP:
                        targetNode = node.TopNode;
                        if (targetNode != null && m_AllNodes[targetNode].Root != nodeInfo.Root)
                            return targetNode;
                        break;
                    case Constant.BOTTOM:
                        targetNode = node.BottomNode;
                        if (targetNode != null && m_AllNodes[targetNode].Root != nodeInfo.Root)
                            return targetNode;
                        break;
                }
            }
            return null;
        }
    }
}
