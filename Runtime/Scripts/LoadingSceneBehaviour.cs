using long18.SceneLoader.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using long18.ExtensionsCore.AssetReferences;
using long18.SceneLoader.ScriptableObjects.Events;
using long18.ExtensionsCore.Events.ScriptableObjects;


#if UNITY_EDITOR
using UnityEditor;
using long18.ExtensionsCore.Editor.Helpers;
#endif

namespace long18.SceneLoader
{
    /// <summary>
    /// Add this to your loading scene so it will unload it self after loading is done.
    /// You can override this for your own loading scene behaviour if you dont want it 
    /// to unload right after scene loaded but after all asset loaded
    /// </summary>
    public class LoadingSceneBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneSO _thisSceneSO;

        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

        [Header("Raise")]
        [SerializeField] private SceneEventChannelSO _unloadSceneEvent;

        protected virtual void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += UnloadThisScene;
        }

        protected virtual void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= UnloadThisScene;
        }

        protected virtual void UnloadThisScene()
        {
            _unloadSceneEvent.RaiseEvent(_thisSceneSO);
        }
    }
}