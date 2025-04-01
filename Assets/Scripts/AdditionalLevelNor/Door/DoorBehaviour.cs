using UnityEngine;
using Unity.Netcode;

public class DoorBehaviour : NetworkBehaviour
{
    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;

        transform.Rotate(90f, 0f, 0f); // Y pour rotation de porte horizontale
        isOpen = true;
    }
}
