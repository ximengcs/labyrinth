using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// 深度优先寻路算法
    /// </summary>
    public class DeepSearchFindAlgorithm : IWayFindAlgorithm
    {
        private Dictionary<Node, PassInfo> m_Infos;
        private List<Path> m_Result;
        private Path m_Current;

        private class PassInfo
        {
            public PassState Left;
            public PassState Right;
            public PassState Top;
            public PassState Bottom;
        }

        private enum PassState
        {
            Uncheck,
            Pass,
            Block
        }

        public Path Execute(Node node)
        {
            m_Infos = new Dictionary<Node, PassInfo>();
            m_Result = new List<Path>();
            m_Current = new Path(node);
            Recursive(node);
            Path result = null;
            foreach (Path path in m_Result)
            {
                if (result == null || result.Step > path.Step)
                    result = path;
            }
            return result;
        }

        private void Recursive(Node node)
        {
            m_Current.Add(node);

            if (!m_Infos.TryGetValue(node, out PassInfo info))
            {
                info = new PassInfo();
                m_Infos.Add(node, info);
            }

            if (info.Left == PassState.Uncheck)
            {
                if (Check(node.LeftSide, node.LeftNode))
                    info.Left = PassState.Pass;
                else
                    info.Left = PassState.Block;
            }
            if (info.Right == PassState.Uncheck)
            {
                if (Check(node.RightSide, node.RightNode))
                    info.Right = PassState.Pass;
                else
                    info.Right = PassState.Block;
            }
            if (info.Top == PassState.Uncheck)
            {
                if (Check(node.TopSide, node.TopNode))
                    info.Top = PassState.Pass;
                else
                    info.Top = PassState.Block;
            }
            if (info.Bottom == PassState.Uncheck)
            {
                if (Check(node.BottomSide, node.BottomNode))
                    info.Bottom = PassState.Pass;
                else
                    info.Bottom = PassState.Block;
            }
            m_Current.RemoveLast();
        }

        private bool Check(Side side, Node node)
        {
            if (m_Current.Contains(node))
                return false;
            switch (side)
            {
                case Side.Destination:
                    m_Result.Add(m_Current.Clone());
                    return true;

                case Side.Pass:
                    if (node != null)
                        Recursive(node);
                    return true;

                case Side.Block:
                    return false;

                default:
                    return false;
            }
        }
    }
}
