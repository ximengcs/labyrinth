
namespace Labyrinth.Core
{
    /// <summary>
    /// 迷宫节点视图
    /// </summary>
    public class NodeView
    {
        public bool Left;
        public bool Right;
        public bool Top;
        public bool Bottom;

        private MapView m_MapView;
        private Node m_Node;
        public Node Node => m_Node;

        public MapView MapView => m_MapView;

        public NodeView LeftView => m_MapView.GetView(m_Node.LeftNode);
        public NodeView RightView => m_MapView.GetView(m_Node.RightNode);
        public NodeView TopView => m_MapView.GetView(m_Node.TopNode);
        public NodeView BottomView => m_MapView.GetView(m_Node.BottomNode);

        public void OnInit(MapView mapView, Node node)
        {
            m_MapView = mapView;
            m_Node = node;
            Refresh();
        }

        public void Refresh()
        {
            Left = m_Node.LeftSide == Side.Pass || m_Node.LeftSide ==  Side.Destination;
            Right = m_Node.RightSide == Side.Pass || m_Node.RightSide ==  Side.Destination;
            Top = m_Node.TopSide == Side.Pass || m_Node.TopSide ==  Side.Destination;
            Bottom = m_Node.BottomSide == Side.Pass || m_Node.BottomSide ==  Side.Destination;
        }
    }
}
