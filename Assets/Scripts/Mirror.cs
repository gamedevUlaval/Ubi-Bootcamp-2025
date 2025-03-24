using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material baseMaterial;
    public Material foggyMaterial;

    void Start()
    {
        meshRenderer.SetMaterials(new List<Material> { baseMaterial});
    }
    
    [ContextMenu("Fog up mirror")]
    public void OnInteraction()
    {
        meshRenderer.SetMaterials(new List<Material> { foggyMaterial});
    }
}
