using System;
using Labyrinth.Utility;
using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// �Թ���ͼ������
    /// </summary>
    public class Generator : SingletonMono<Generator>
    {
        private Dictionary<int, Map> m_Maps;

        #region Event
        public event Action<Map> MapCreateEvent;
        public event Action<Map> MapDestoryEvent;
        #endregion

        private void Start()
        {
            m_Maps = new();
        }

        /// <summary>
        /// �����Թ�
        /// </summary>
        /// <param name="width">���</param>
        /// <param name="height">�߶�</param>
        /// <returns>��ͼ��ͼʵ��</returns>
        public MapView CreateMap(int width, int height)
        {
            Map map = new Map();
            map.OnInit(m_Maps.Count, width, height);
            m_Maps.Add(map.Id, map);

            MapView mapView = new MapView();
            mapView.OnInit(map);

            MapCreateEvent?.Invoke(map);
            return mapView;
        }

        /// <summary>
        /// �����Թ���ͼ
        /// </summary>
        /// <param name="map">��ͼʵ��</param>
        public void DestroyMap(Map map)
        {
            if (m_Maps.ContainsKey(map.Id))
            {
                MapDestoryEvent?.Invoke(map);
                map.OnDispose();
                m_Maps.Remove(map.Id);
            }
        }
    }
}