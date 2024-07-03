using System;
using Labyrinth.Utility;
using System.Collections.Generic;

namespace Labyrinth.Core
{
    /// <summary>
    /// 迷宫地图生成器
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
        /// 生成迷宫
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>地图视图实例</returns>
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
        /// 销毁迷宫地图
        /// </summary>
        /// <param name="map">地图实例</param>
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