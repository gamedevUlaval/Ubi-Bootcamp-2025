using UnityEngine;

using FishNet;


public class MultiplayerStarter : MonoBehaviour
{
    // Démarre le mode hôte (serveur et client dans la même instance)
    public void StartHost()
    {
        Debug.Log("Démarrage du host via Steam P2P...");
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.StartConnection();

        if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.StartConnection();
    }

    // Démarre en mode client uniquement et force la connexion en mode P2P
    public void StartClient()
    {
         if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.StartConnection();
    }
}
