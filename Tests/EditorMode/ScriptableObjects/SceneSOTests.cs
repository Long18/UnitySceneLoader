using NUnit.Framework;
using long18.SceneLoader.ScriptableObjects;

namespace long18.SceneLoader.Tests.ScriptableObjects
{
    [TestFixture, Category("Smoke Tests")]
    public class SceneSOTests
    {
        [Test]
        public void SceneSOs_CreatedCorrectly()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:SceneSO");

            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var sceneSO = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneSO>(path);
                Assert.IsNotEmpty(sceneSO.SceneReference.AssetGUID,
                    $"{path} has no scene asset reference.");
            }
        }
    }
}