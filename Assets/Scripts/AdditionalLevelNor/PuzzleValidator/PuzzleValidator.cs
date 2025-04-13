using UnityEngine;
using Unity.Netcode;
public class PuzzleValidator : NetworkBehaviour
{
    public ObjectsCheck[] objectsToCheck;
    public DoorBehaviour doorBehaviour;
    public static PuzzleValidator Instance { get; private set;}

    void Awake()
    {
        
        if (Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        else
            {
                Destroy(gameObject);
                return;
            }
    }
    public void CheckIfSolved()
    {
        bool allCorrect = true;

        foreach (var obj in objectsToCheck)
        {
            StartCoroutine(obj.TurnOnStatusLight());

            if (!obj.IsCorrectlyPlaced())
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            OpenDoorRpc();
        }
        else
        {
            Debug.Log("Objets pas au bon endroit!");
            // Mettre un son comme quoi ça ne marche pas ou autre
        }
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void OpenDoorRpc()
    {
        SoundManager.Instance.PlaySuccessMusic();
        KeyManager.Instance.AddKey(2);
    }
    
}
