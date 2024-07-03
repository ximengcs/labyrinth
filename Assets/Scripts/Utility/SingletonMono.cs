using UnityEngine;

namespace Labyrinth.Utility
{
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T s_Instance;
        public static T Inst => s_Instance;

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this as T;
        }
    }
}
