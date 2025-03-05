using Steamworks;
using UnityEngine;

public class SteamManage : MonoBehaviour
{
    private void Start()
    {
        if (!SteamAPI.Init())
        {
            Debug.LogError("Steam API failed to initialize.");
            return;
        }
        Debug.Log("Steam initialized with ID: " + SteamUser.GetSteamID());
    }

    private void OnDestroy()
    {
        SteamAPI.Shutdown();
    }
}
