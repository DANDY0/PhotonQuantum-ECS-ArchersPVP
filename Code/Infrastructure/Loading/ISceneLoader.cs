using System;
using System.Threading.Tasks;

namespace Code.Infrastructure.Loading
{
    public interface ISceneLoader
    {
        public void LoadScene(SceneId sceneId, Action onLoaded = null);
        public void LoadSceneAdditive(SceneId sceneId, Action onLoaded = null);
        // public void UnloadSceneAsync(SceneId sceneId);
        public Task UnloadSceneAsync(SceneId sceneId);

    }
}