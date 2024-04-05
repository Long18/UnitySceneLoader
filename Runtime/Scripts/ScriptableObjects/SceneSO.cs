using System;
using long18.ExtensionsCore.AssetReferences;
using long18.ExtensionsCore.ScriptableObjects;
using UnityEngine;

namespace long18.SceneLoader.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SceneSO", menuName = "long18/Scene Manager/Scene SO")]
    public class SceneSO : SerializableScriptableObject
    {
        [field: SerializeField]
        public SceneAssetReference SceneReference { get; private set; }

        [Tooltip("These scene will be loaded first when the scene is loaded")]
        [field: SerializeField]
        public SceneSO[] DependentScenes { get; private set; } = Array.Empty<SceneSO>();
    }
}