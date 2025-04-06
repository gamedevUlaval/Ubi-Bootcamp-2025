using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class LightLamps : NetworkBehaviour, IInteractable
{
    [SerializeField] private float delay = 10.0f;
    [SerializeField] private GameObject[] lamps;
    private bool isPlaying = false;

    public InteractableType InteractableType => InteractableType.Cooldown;

    public void Interact()
    {
        StartSequenceRpc();
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        throw new System.NotImplementedException();
    }
    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void StartSequenceRpc()
    {
        if (!isPlaying)
        {
            StartCoroutine(ShowSequence());
        }
    }

    IEnumerator ShowSequence()
    {
        isPlaying = true;
        for (int i = 0; i < lamps.Length; i++)
        {
            Renderer _renderer = lamps[i].GetComponent<Renderer>();
            Color defaultcolor = _renderer.material.color;
            _renderer.material.SetColor("_BaseColor", Color.yellow);
            yield return new WaitForSeconds(delay);
            _renderer.material.SetColor("_BaseColor", defaultcolor);
        }
        isPlaying = false;
    }
}
