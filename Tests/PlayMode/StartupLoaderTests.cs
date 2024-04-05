using System.Collections;
using long18.ExtensionsCore.Editor.Helpers;
using long18.ExtensionsCore.Events.ScriptableObjects;
using long18.SceneLoader.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace long18.SceneLoader.Tests
{
    [TestFixture, Category("Integration Tests")]
    public class StartupLoaderTests
    {
        private const string ASSET_FOLDER_PATH = "Packages/long18.scene-loader";
        private const string STARTUP_SCENE_NAME = "StartupScene";
        private const string STARTUP_PREFAB = "StartupLoader";
        private const string SCENE_MANAGER_SO = "SceneManagerSO";
        private const string SCENE_MANAGER_GUID = "c93932688e24e0843b61b851607ef2c6";
        private const string FIRST_SCENE_GUID = "a88f1ce33dcfd3c4d8f131e7c5370cf1";
        private const string SCENE_LOADED_EVENT = "SceneLoadedEvent";

        private string _firstSceneSOPath = $"{ASSET_FOLDER_PATH}/FirstSceneSO.asset";
        private SceneSO _firstSceneSO;
        private SceneSO _sceneManagerSO;
        private VoidEventChannelSO _sceneLoadedEvent;
        private GameObject _startupLoaderObject;

        private bool _isSceneLoaded;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _sceneLoadedEvent = AssetFinder.FindAssetWithNameInPath<VoidEventChannelSO>(
                SCENE_LOADED_EVENT, ASSET_FOLDER_PATH);
            _sceneLoadedEvent.EventRaised += OnSceneLoaded;

            _firstSceneSO = SceneTestHelper.InitSceneSO(FIRST_SCENE_GUID);
            AssetDatabase.DeleteAsset(_firstSceneSOPath);
            AssetDatabase.CreateAsset(_firstSceneSO, _firstSceneSOPath);
            AssetDatabase.SaveAssets();

            _sceneManagerSO = AssetFinder.FindAssetWithNameInPath<SceneManagerSO>(
                SCENE_MANAGER_SO, ASSET_FOLDER_PATH);
            _startupLoaderObject = AssetFinder.FindAssetWithNameInPath<GameObject>(
                STARTUP_PREFAB, ASSET_FOLDER_PATH);
            var startupLoader = _startupLoaderObject.GetComponent<StartupLoader>();
            startupLoader.SetPrivateProperty("_firstSceneSO", _firstSceneSO);

            SceneManager.LoadScene(STARTUP_SCENE_NAME);
        }

        private void OnSceneLoaded()
        {
            _isSceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator StartupLoader_LoadsStartupScene_FirstSceneAndManagerLoaded()
        {
            yield return new WaitUntil(() => _isSceneLoaded);
            Assert.IsTrue(_isSceneLoaded);
            AssertSceneLoadedProperly(_firstSceneSO, FIRST_SCENE_GUID);
            AssertSceneLoadedProperly(_sceneManagerSO, SCENE_MANAGER_GUID);
        }

        private void AssertSceneLoadedProperly(SceneSO sceneSO, string guid)
        {
            var loadedScene = SceneManager.GetSceneByName(sceneSO.SceneReference.editorAsset.name);
            Assert.IsTrue(loadedScene.isLoaded);
            var scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Assert.AreEqual(loadedScene.path, scenePath);
        }

        [TearDown]
        public void TearDown()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
            _isSceneLoaded = false;
            AssetDatabase.DeleteAsset(_firstSceneSOPath);
            var startupLoader = _startupLoaderObject.GetComponent<StartupLoader>();
            startupLoader.SetPrivateProperty("_firstSceneSO", null);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.RemoveGroup(settings.FindGroup(SceneTestHelper.TEST_GROUP));
            _sceneManagerSO.SceneReference.ReleaseAsset();
        }
    }
}