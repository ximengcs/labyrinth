using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Labyrinth.Core;

namespace Labyrinth.UIElements
{
    /// <summary>
    /// 控制UI
    /// </summary>
    public class ControlUI : MonoBehaviour
    {
        public TMP_InputField WidthInput;
        public TMP_InputField HeightInput;
        public Button GenerateBtn;
        public Button ResetBtn;
        public TextMeshProUGUI GenerateTime;
        public TextMeshProUGUI WayFindTime;

        private MapView m_CurrentMap;

        private void Start()
        {
            GenerateBtn.onClick.AddListener(ClickGenerateHandler);
            ResetBtn.onClick.AddListener(ClickResetHandler);
        }

        private void Update()
        {
            if (m_CurrentMap == null)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int width = m_CurrentMap.Map.Width;
                int height = m_CurrentMap.Map.Height;
                worldPos += new Vector2(width, height);
                int x = (int)(worldPos.x / 2);
                int y = (int)(worldPos.y / 2);
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    NodeView view = m_CurrentMap.GetView(x, y);
                    MapView mapView = view.MapView;

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    IWayFindAlgorithm finder = new DeepSearchFindAlgorithm();
                    Path path = finder.Execute(view.Node);
                    sw.Stop();
                    SetWayFindTime(sw.ElapsedMilliseconds);
                    if (path != null)
                    {
                        mapView.ResetState();
                        mapView.PaintPath(path);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            GenerateBtn.onClick.RemoveListener(ClickGenerateHandler);
            ResetBtn.onClick.RemoveListener(ClickResetHandler);
        }

        private void ClickGenerateHandler()
        {
            if (int.TryParse(HeightInput.text, out int height) && int.TryParse(WidthInput.text, out int width))
            {
                if (m_CurrentMap != null)
                    Generator.Inst.DestroyMap(m_CurrentMap.Map);
                m_CurrentMap = Generator.Inst.CreateMap(width, height);
                m_CurrentMap.Map.RefreshEvent += SetGenerateTime;
                m_CurrentMap.Map.Reset();
            }
        }

        private void ClickResetHandler()
        {
            m_CurrentMap.Map.Reset();
        }

        private void SetGenerateTime(long time)
        {
            GenerateTime.text = $"{time}ms";
        }

        private void SetWayFindTime(long time)
        {
            WayFindTime.text = $"{time}ms";
        }
    }
}
