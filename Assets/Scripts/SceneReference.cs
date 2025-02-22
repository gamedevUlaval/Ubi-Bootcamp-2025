#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class SceneReference
{
    #if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
    #endif

    // Ce champ est utilisé à l'exécution pour stocker le nom de la scène.
    [SerializeField] private string sceneName;

    public string SceneName
    {
        get
        {
            #if UNITY_EDITOR
            if (sceneAsset != null)
            {
                sceneName = sceneAsset.name;
            }
            #endif
            return sceneName;
        }
    }
}
