using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Material baseMaterial;
    public Material foggyMaterial;
    public MeshRenderer mirrorMeshRenderer;
    void Start()
    {
        mirrorMeshRenderer.material = baseMaterial;
    }
    
    [ContextMenu("Fog up mirror")]
    void OnInteraction()
    {
        mirrorMeshRenderer.material = foggyMaterial;
    }

}
