using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Loading
{
public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public event Action<SceneId> OnSceneLoaded;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(SceneId sceneId, Action onLoaded = null)
        {
            Debug.Log($"[SceneLoader] Starting load of scene: {sceneId}");
            _coroutineRunner.StartCoroutine(Load(sceneId, onLoaded));
        }

        public void LoadSceneAdditive(SceneId sceneId, Action onLoaded = null)
        {
            Debug.Log($"[SceneLoader] Starting additive load of scene: {sceneId}");
            _coroutineRunner.StartCoroutine(LoadAdditive(sceneId, onLoaded));
        }

        public Task UnloadSceneAsync(SceneId sceneId)
        {
            string name = sceneId.ToString();
            var scene = SceneManager.GetSceneByName(name);

            if (!scene.isLoaded)
            {
                Debug.LogWarning($"[SceneLoader] Attempted to unload scene '{sceneId}' but it is not loaded.");
                return Task.CompletedTask;
            }

            Debug.Log($"[SceneLoader] Starting unload of scene: {sceneId}");

            var tcs = new TaskCompletionSource<bool>();

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(name);
            if (asyncUnload == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start unloading scene '{sceneId}'.");
                tcs.SetResult(false);
                return tcs.Task;
            }

            asyncUnload.completed += _ =>
            {
                Debug.Log($"[SceneLoader] Scene '{sceneId}' unloaded successfully.");
                tcs.SetResult(true);
            };

            return tcs.Task;
        }

        private IEnumerator Load(SceneId sceneId, Action onLoaded)
        {
            string name = sceneId.ToString();

            if (SceneManager.GetActiveScene().name == name)
            {
                Debug.LogWarning($"[SceneLoader] Scene '{sceneId}' is already active. Skipping load.");
                onLoaded?.Invoke();
                OnSceneLoaded?.Invoke(sceneId);
                yield break;
            }

            var waitNextScene = SceneManager.LoadSceneAsync(name);
            if (waitNextScene == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start loading scene '{sceneId}'.");
                yield break;
            }

            while (!waitNextScene.isDone)
                yield return null;

            Debug.Log($"[SceneLoader] Scene '{sceneId}' loaded successfully.");
            onLoaded?.Invoke();
            OnSceneLoaded?.Invoke(sceneId);
        }

        private IEnumerator LoadAdditive(SceneId sceneId, Action onLoaded)
        {
            string name = sceneId.ToString();

            if (SceneManager.GetSceneByName(name).isLoaded)
            {
                Debug.LogWarning($"[SceneLoader] Scene '{sceneId}' is already loaded (additive). Skipping.");
                onLoaded?.Invoke();
                OnSceneLoaded?.Invoke(sceneId);
                yield break;
            }

            var asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            if (asyncLoad == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start additive loading of scene '{sceneId}'.");
                yield break;
            }

            while (!asyncLoad.isDone)
                yield return null;

            Debug.Log($"[SceneLoader] Scene '{sceneId}' loaded additively.");
            onLoaded?.Invoke();
            OnSceneLoaded?.Invoke(sceneId);
        }

        private IEnumerator Unload(SceneId sceneId)
        {
            string name = sceneId.ToString();

            var asyncUnload = SceneManager.UnloadSceneAsync(name);
            if (asyncUnload == null)
            {
                Debug.LogError($"[SceneLoader] Failed to start unloading scene '{sceneId}'.");
                yield break;
            }

            while (!asyncUnload.isDone)
                yield return null;

            Debug.Log($"[SceneLoader] Scene '{sceneId}' unloaded successfully.");
        }
    }
    public enum SceneId
    {
        None = 0,
        Boot = 1,
        StartConnection = 2,
        Menu = 3,
        Battle = 4,
    }
}
