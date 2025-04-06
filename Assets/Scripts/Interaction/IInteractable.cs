using UnityEngine;

public interface IInteractable
{
    void Interact();
    bool InteractWith(GameObject tryToInteractWith);
    InteractableType InteractableType { get; }
}

public enum InteractableType
{
    Pickable,
    Static,
    Cooldown,
}
