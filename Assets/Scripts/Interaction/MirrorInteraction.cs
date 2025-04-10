using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MirrorInteraction : NetworkBehaviour, IInteractable
{
    public MeshRenderer meshRenderer;
    public Material baseMaterial;
    public Material foggyMaterial;

    [SerializeField] private AudioClip ghostHaunt;

    void Start()
    {
        meshRenderer.SetMaterials(new List<Material> { baseMaterial});
    }
    
    [ContextMenu("Fog up mirror")]
    public void Interact()
    {
        
        InteractionRpc();
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    [Rpc(SendTo.Everyone)]
    private void InteractionRpc()
    {
        SoundManager.Instance.PlayGhostHaunt();
        meshRenderer.SetMaterials(new List<Material> { foggyMaterial});
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}
