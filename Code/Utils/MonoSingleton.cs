using UnityEngine;
namespace Code.Utils
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool _dontDestroyOnLoad = false;
        
        private static T _instance;
        private static readonly object Lock = new object();


        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        Debug.LogError($"[MonoSingleton] Instance of {typeof(T)} not found in the scene.");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = this as T;

                    if (_dontDestroyOnLoad)
                    {
                        DontDestroyOnLoad(gameObject);
                    }
                }
                else if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}