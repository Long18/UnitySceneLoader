using long18.SceneLoader.ScriptableObjects.Events;
using long18.SceneLoader.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using long18.ExtensionsCore.Editor.Helpers;
#endif
using System.Linq;
using Cysharp.Threading.Tasks;

namespace long18.SceneLoader.Editor
{
    public class ColdBoot : MonoBehaviour
    {
        [SerializeField] private SceneSO _thisSceneSO;
        [SerializeField] private SceneSO _sceneManagerSO;
        [SerializeField] private SceneEventChannelSO _loadSceneEvent;

        private bool _isColdBoot;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            if (_thisSceneSO != null &&
                _thisSceneSO.SceneReference.editorAsset.name == gameObject.scene.name) return;

            var allSceneSOs = AssetFinder.FindAssetsWithType<SceneSO>();
            _thisSceneSO = allSceneSOs
                .Where(x => x.SceneReference.editorAsset.name == gameObject.scene.name)
                .FirstOrDefault();
        }

        private async UniTask Awake()
        {
            _isColdBoot =
                !SceneManager.GetSceneByName(_sceneManagerSO.SceneReference.editorAsset.name).isLoaded
                && !_sceneManagerSO.SceneReference.OperationHandle.IsValid();
            if (!_isColdBoot) return;

            await _sceneManagerSO.SceneReference.TryLoadScene(LoadSceneMode.Single);
            _loadSceneEvent.RaiseEvent(_thisSceneSO);
        }
#endif
    }
}