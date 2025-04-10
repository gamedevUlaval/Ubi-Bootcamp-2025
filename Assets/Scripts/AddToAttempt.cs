using Unity.Netcode;
using UnityEngine;

public class AddToAttempt : NetworkBehaviour
{
    [SerializeField] private LightCombination _lightCombination;
    [SerializeField] private int id;

    [Header("Sound")]
    [SerializeField] private AudioClip buttonSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))
        {
            SoundManager.Instance.PlaySFX(buttonSound);
            _lightCombination.AddAttempt(id);
        }
    }
}
