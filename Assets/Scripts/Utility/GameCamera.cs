
using Labyrinth.Core;
using UnityEngine;

namespace Labyrinth.Utility
{
    public class GameCamera : MonoBehaviour
    {
        private Camera m_Cam;

        private void Start()
        {
            m_Cam = GetComponent<Camera>();
            Generator.Inst.MapCreateEvent += MapCreateHandler;
        }

        private void OnDestroy()
        {
            Generator.Inst.MapCreateEvent -= MapCreateHandler;
        }

        private void MapCreateHandler(Map map)
        {
            m_Cam.orthographicSize = map.Height + map.Height * 0.1f;
        }
    }
}
