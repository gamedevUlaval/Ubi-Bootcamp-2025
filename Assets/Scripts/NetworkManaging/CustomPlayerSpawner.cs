using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet;

public class CustomPlayerSpawner : NetworkBehaviour
{
    public static CustomPlayerSpawner Instance { get; private set; }

    [Header("Références aux prefabs de joueur")]
    public GameObject playerPrefabGhost;
    public GameObject playerPrefabHuman;
    [Header("SpawnPoints")]
    public Transform[] spawnPoints;

    // Stocke la sélection de rôle de chaque client (0 : Ghost, 1 : Human)
    private Dictionary<int, int> clientRoleSelections = new Dictionary<int, int>();

    // Stocke les connexions en attente de sélection
    private Dictionary<int, NetworkConnection> pendingConnections = new Dictionary<int, NetworkConnection>();

    // Nombre de joueurs spawnés (limité à 2 ici)
    private int playerCount = 0;
    private int maxPlayers = 2;

    public override void OnStartServer()
    {
        base.OnStartServer();
        // Abonnement à l'événement de connexion des clients
        InstanceFinder.ServerManager.OnRemoteConnectionState += OnPlayerConnected;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.OnRemoteConnectionState -= OnPlayerConnected;
    }

    // Lorsqu'un client se connecte
    private void OnPlayerConnected(NetworkConnection conn, RemoteConnectionStateArgs state)
    {
        if (state.ConnectionState == RemoteConnectionState.Started)
        {
            // Abonnez-vous à l'événement OnLoadedStartScenes pour attendre que le client charge la scène
            conn.OnLoadedStartScenes += OnPlayerSceneLoaded;
        }
    }

    // Appelé quand le client a chargé ses scènes initiales
    private void OnPlayerSceneLoaded(NetworkConnection conn, bool asServer)
    {
        // Se désabonner pour éviter plusieurs appels
        conn.OnLoadedStartScenes -= OnPlayerSceneLoaded;
        // Ajouter la connexion aux connexions en attente de sélection de rôle
        pendingConnections[conn.ClientId] = conn;
        Debug.Log($"Le client {conn.ClientId} a chargé la scène et attend la sélection de rôle.");
    }

    // Cette méthode sera appelée par l'UI lorsque le joueur clique sur un bouton de sélection
    public void OnSelectRole(int role)
    {
        // Récupère l'ID du client local
        int clientId = InstanceFinder.ClientManager.Connection.ClientId;
        // Appelle le ServerRpc pour transmettre la sélection au serveur
        RequestPrefabSelectionServerRpc(role, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestPrefabSelectionServerRpc(int role, int clientId)
    {
        if (pendingConnections.TryGetValue(clientId, out NetworkConnection conn))
        {
            // Enregistre la sélection pour suivi
            clientRoleSelections[clientId] = role;
            // Spawner le joueur selon le rôle choisi
            SpawnPlayer(conn, role);
            // Retire la connexion des connexions en attente
            pendingConnections.Remove(clientId);
        }
        else
        {
            Debug.LogWarning($"Aucune connexion en attente trouvée pour le client {clientId}.");
        }
    }

    // Choisit le prefab en fonction du rôle et spawne le joueur
    private void SpawnPlayer(NetworkConnection conn, int role)
    {
        int spawnIndex = 0;
        if (playerCount >= maxPlayers)
        {
            Debug.LogWarning("Nombre maximum de joueurs atteint.");
            return;
        }

        // 0 pour Ghost, 1 pour Human
        GameObject prefabToSpawn = (role == 0) ? playerPrefabGhost : playerPrefabHuman;

        if (prefabToSpawn == null)
        {
            Debug.LogError("Le prefab correspondant n'est pas assigné !");
            return;
        }

        // Choisir un spawn point en fonction du nombre de joueurs déjà spawnés
        if (spawnPoints != null && prefabToSpawn == playerPrefabGhost)
        {
            spawnIndex = 0;
            
        }else
        {
            spawnIndex = 1;
        }
        prefabToSpawn.transform.position = spawnPoints[spawnIndex].position;
        prefabToSpawn.transform.rotation = spawnPoints[spawnIndex].rotation;

        // Instanciation côté serveur et spawn via FishNet
        GameObject playerInstance = Instantiate(prefabToSpawn);
        InstanceFinder.ServerManager.Spawn(playerInstance, conn);

        playerCount++;
        Debug.Log($"Joueur spawné: {playerInstance.name} pour le client {conn.ClientId} (Total joueurs: {playerCount}).");
    }
}
