using UnityEngine;

[CreateAssetMenu(fileName = "InteractibleItemData", menuName = "Scriptable Objects/InteractibleItemData")]
public class InteractableItemData : ScriptableObject
{
    public string itemName;
    public string interactionPrompt;
}
