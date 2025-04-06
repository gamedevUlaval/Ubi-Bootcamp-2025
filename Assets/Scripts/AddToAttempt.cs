using Unity.Netcode;
using UnityEngine;

public class AddToAttempt : NetworkBehaviour
{
    [SerializeField] private LightCombination _lightCombination;
    [SerializeField] private int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            _lightCombination.AddAttempt(id);
        }
    }
}
