using UnityEngine;
using FishNet.Managing;
using FishNet; 
using FishySteamworks;
public class MultiplayerStarter : MonoBehaviour
{
    public void StartHost()
    {
        Debug.Log("Démarrage du serveur en mode Host via Steam P2P...");
        
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.StartConnection();

        if (InstanceFinder.ClientManager != null)
            InstanceFinder.ClientManager.StartConnection();
    }

    public void StartClient()
    {
        Debug.Log("Tentative de connexion via Steam P2P...");

        if (InstanceFinder.ClientManager != null)
        {
        

            InstanceFinder.ClientManager.StartConnection();
        }
    }
}
