using UnityEngine;
using FishNet.Managing;
using FishNet;

public class MultiplayerStarter : MonoBehaviour
{
    // Démarre le mode hôte (serveur et client)
    public void StartHost()
    {
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.StartConnection();
        
        if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.StartConnection();
    }

    // Démarre en mode client uniquement
    public void StartClient()
    {
        if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.StartConnection();
    }
}