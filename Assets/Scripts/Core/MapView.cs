using UnityEngine;
using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// 迷宫视图
    /// </summary>
    public class MapView
    {
        private GameObject m_Inst;
        private SpriteRenderer m_Background;
        private Map m_Map;
        private Texture2D m_Tex;
        private List<NodeView> m_Nodes;

        public Map Map => m_Map;

        public void OnInit(Map map)
        {
            m_Map = map;
            m_Inst = new GameObject($"{nameof(MapView)}_{m_Map.Id}");
            m_Inst.transform.localScale = Vector3.one * 100;
            m_Background = m_Inst.AddComponent<SpriteRenderer>();
            m_Tex = new Texture2D(m_Map.Width * 2 + 1, m_Map.Height * 2 + 1);
            m_Tex.filterMode = FilterMode.Point;
            m_Background.sprite = Sprite.Create(m_Tex, new Rect(0, 0, m_Tex.width, m_Tex.height), new Vector2(0.5f, 0.5f));

            m_Nodes = new List<NodeView>();
            foreach (Node node in m_Map.Nodes)
            {
                NodeView nodeView = new NodeView();
                nodeView.OnInit(this, node);
                m_Nodes.Add(nodeView);
            }
            ResetState();
            m_Map.RefreshEvent += MapModelRefreshHandler;
            m_Map.DisposeEvent += MapModelDisposeHandler;
        }

        public NodeView GetView(Node node)
        {
            if (node == null) return null;
            return m_Nodes[node.Pos.x * m_Map.Width + node.Pos.y];
        }

        public NodeView GetView(int x, int y)
        {
            return m_Nodes[x * m_Map.Width + y];
        }

        public void ResetState()
        {
            ClearColor();
            foreach (NodeView view in m_Nodes)
            {
                view.Refresh();
                int x = view.Node.Pos.x;
                int y = view.Node.Pos.y;

                if (view.Left)
                {
                    switch (view.Node.LeftSide)
                    {
                        case Side.Pass: SetNodeLeftColor(x, y, Color.cyan); break;
                        case Side.Destination: SetNodeLeftColor(x, y, Color.red); break;
                    }
                }
                if (view.Right)
                {
                    switch (view.Node.RightSide)
                    {
                        case Side.Pass: SetNodeRightColor(x, y, Color.cyan); break;
                        case Side.Destination: SetNodeRightColor(x, y, Color.red); break;
                    }
                }
                if (view.Top)
                {
                    switch (view.Node.TopSide)
                    {
                        case Side.Pass: SetNodeTopColor(x, y, Color.cyan); break;
                        case Side.Destination: SetNodeTopColor(x, y, Color.red); break;
                    }
                }
                if (view.Bottom)
                {
                    switch (view.Node.BottomSide)
                    {
                        case Side.Pass: SetNodeBottomColor(x, y, Color.cyan); break;
                        case Side.Destination: SetNodeBottomColor(x, y, Color.red); break;
                    }
                }

                m_Tex.SetPixel(x * 2 + 1, y * 2 + 1, Color.cyan);
            }
            m_Tex.Apply();
        }

        private void SetNodeLeftColor(int x, int y, Color color)
        {
            m_Tex.SetPixel(x * 2, y * 2 + 1, color);
        }

        private void SetNodeRightColor(int x, int y, Color color)
        {
            m_Tex.SetPixel(x * 2 + 2, y * 2 + 1, color);
        }

        private void SetNodeTopColor(int x, int y, Color color)
        {
            m_Tex.SetPixel(x * 2 + 1, y * 2 + 2, color);
        }

        private void SetNodeBottomColor(int x, int y, Color color)
        {
            m_Tex.SetPixel(x * 2 + 1, y * 2, color);
        }

        public void PaintPath(Path path)
        {
            NodeView view = GetView(path.StartNode);
            foreach (Node node in path)
            {
                NodeView curView = GetView(node);
                m_Tex.SetPixel(curView.Node.Pos.x * 2 + 1, curView.Node.Pos.y * 2 + 1, Color.yellow);
                if (view.LeftView != null && view.LeftView.Node == node)
                {
                    Node tmp = view.LeftView.Node;
                    SetNodeRightColor(tmp.Pos.x, tmp.Pos.y, Color.yellow);
                    SetNodeLeftColor(view.Node.Pos.x, view.Node.Pos.y, Color.yellow);
                    view = curView;
                    continue;
                }

                if (view.RightView != null && view.RightView.Node == node)
                {
                    Node tmp = view.RightView.Node;
                    SetNodeLeftColor(tmp.Pos.x, tmp.Pos.y, Color.yellow);
                    SetNodeRightColor(view.Node.Pos.x, view.Node.Pos.y, Color.yellow);
                    view = curView;
                    continue;
                }

                if (view.TopView != null && view.TopView.Node == node)
                {
                    Node tmp = view.TopView.Node;
                    SetNodeBottomColor(tmp.Pos.x, tmp.Pos.y, Color.yellow);
                    SetNodeTopColor(view.Node.Pos.x, view.Node.Pos.y, Color.yellow);
                    view = curView;
                    continue;
                }

                if (view.BottomView != null && view.BottomView.Node == node)
                {
                    Node tmp = view.BottomView.Node;
                    SetNodeTopColor(tmp.Pos.x, tmp.Pos.y, Color.yellow);
                    SetNodeBottomColor(view.Node.Pos.x, view.Node.Pos.y, Color.yellow);
                    view = curView;
                    continue;
                }
            }
            m_Tex.Apply();
        }

        public void ClearColor()
        {
            for (int i = 0; i < m_Tex.height; i++)
                for (int j = 0; j < m_Tex.width; j++)
                    m_Tex.SetPixel(i, j, Color.black);
            m_Tex.Apply();
        }

        private void MapModelRefreshHandler(long time)
        {
            ResetState();
        }

        private void MapModelDisposeHandler()
        {
            m_Map.RefreshEvent -= MapModelRefreshHandler;
            m_Map.DisposeEvent -= MapModelDisposeHandler;
            GameObject.Destroy(m_Inst);
        }
    }
}
