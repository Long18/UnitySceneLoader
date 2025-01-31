using long18.ExtensionsCore.Events.ScriptableObjects;
using long18.SceneLoader.ScriptableObjects;
using long18.SceneLoader.ScriptableObjects.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace long18.SceneLoader
{
    public class SceneUnloader : MonoBehaviour
    {
        [Header("Listen to")]
        [SerializeField] private SceneEventChannelSO _unloadSceneEvent;

        [Header("Raise")]
        [SerializeField] private VoidEventChannelSO _sceneUnloadedEventChannel;

        private void OnEnable()
        {
            _unloadSceneEvent.EventRaised += UnloadSceneRequested;
        }

        private void OnDisable()
        {
            _unloadSceneEvent.EventRaised -= UnloadSceneRequested;
        }

        private void UnloadSceneRequested(SceneSO sceneSO)
        {
            _ = UnloadScene(sceneSO);
        }

        public async UniTask UnloadScene(SceneSO sceneSO)
        {
            if (!sceneSO.SceneReference.OperationHandle.IsValid()) return;
            var handler = Addressables.UnloadSceneAsync(sceneSO.SceneReference.OperationHandle);
            await UniTask.WaitUntil(() => handler.IsDone);
            sceneSO.SceneReference.ReleaseAsset();
            await Resources.UnloadUnusedAssets();
            _sceneUnloadedEventChannel.RaiseEvent();
        }
    }
}