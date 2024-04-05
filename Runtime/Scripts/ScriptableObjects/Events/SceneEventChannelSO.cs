using UnityEngine;
using long18.ExtensionsCore.Events.ScriptableObjects;

namespace long18.SceneLoader.ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "SceneEvent", menuName = "long18/Scene Manager/Events/Scene SO Event Channel")]
    public class SceneEventChannelSO : GenericEventChannelSO<SceneSO>
    {
        protected override void OnRaiseEvent(SceneSO sceneSO)
        {
            if (sceneSO == null)
            {
                Debug.LogWarning("OnRaiseEvent:: Raise Scene event with null");
                return;
            }

            base.OnRaiseEvent(sceneSO);
        }
    }
}