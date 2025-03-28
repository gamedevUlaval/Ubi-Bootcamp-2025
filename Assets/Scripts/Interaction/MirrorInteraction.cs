using System;
using System.Collections.Generic;
using UnityEngine;

public class MirrorInteraction : MonoBehaviour, IInteractable
{
    public MeshRenderer meshRenderer;
    public Material baseMaterial;
    public Material foggyMaterial;

    void Start()
    {
        meshRenderer.SetMaterials(new List<Material> { baseMaterial});
    }
    
    [ContextMenu("Fog up mirror")]
    public void Interact()
    {
        meshRenderer.SetMaterials(new List<Material> { foggyMaterial});
    }
    
    public bool InteractWith(GameObject tryToInteractWith)
    {
        Debug.Log("Should not interact with something in hand");
        return false;
    }
    
    public InteractableType InteractableType => InteractableType.Static;
}
