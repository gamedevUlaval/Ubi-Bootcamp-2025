using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using Unity.Multiplayer.Playmode;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Serialization;

public class ConnectionManager : MonoBehaviour
{
   public string ProfileName;
   public string SessionName;
   private int _maxPlayers = 2;
   public bool GuiEnabled = false;
   private ConnectionState _state = ConnectionState.Disconnected;
   private ISession _session;
   private NetworkManager m_NetworkManager;
   
   public static ConnectionManager Instance { get; private set; }

   private enum ConnectionState
   {
       Disconnected,
       Connecting,
       Connected,
   }

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        m_NetworkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        await UnityServices.InitializeAsync();
        if (CurrentPlayer.ReadOnlyTags().Any(str=>str.Contains("INIT")))
        {
            GuiEnabled = true;
            SessionName = Environment.UserName;//+System.DateTime.Now.ToString("HHmm");
            ProfileName = Environment.UserName + CurrentPlayer.ReadOnlyTags().First();//+System.DateTime.Now.ToString("HHmm");
            if (CurrentPlayer.ReadOnlyTags().Any(str=>str.Contains("GHOST")))
            {
                await Task.Delay(1000);
            }
            await CreateOrJoinSessionAsync();
        }
    }

    private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
    {
        if (m_NetworkManager.LocalClient.IsSessionOwner)
        {
            Debug.Log($"I, Client-{m_NetworkManager.LocalClientId}, am the session owner!");
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (m_NetworkManager.LocalClientId == clientId)
        {
            Debug.Log($"This Client-{clientId} is connected and can spawn {nameof(NetworkObject)}s.");
        }
        else
        {
            Debug.Log($"Other client-{clientId} is connected and can spawn {nameof(NetworkObject)}s.");
        }
    }

   private void OnGUI()
   {
       if (!GuiEnabled)
           return;
       if (_state == ConnectionState.Connected)
           return;

       GUI.enabled = _state != ConnectionState.Connecting;

       using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
       {
           GUILayout.Label("Profile Name", GUILayout.Width(100));
           ProfileName = GUILayout.TextField(ProfileName);
       }

       using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
       {
           GUILayout.Label("Session Name", GUILayout.Width(100));
           SessionName = GUILayout.TextField(SessionName);
       }

       GUI.enabled = GUI.enabled && !string.IsNullOrEmpty(ProfileName) && !string.IsNullOrEmpty(SessionName);

       if (GUILayout.Button("Create or Join Session"))
       {
           CreateOrJoinSessionAsync();
       }
   }

   private void OnDestroy()
   {
       _session?.LeaveAsync();
   }

   public async Task CreateOrJoinSessionAsync()
   {
       _state = ConnectionState.Connecting;

       try
       {
           if (AuthenticationService.Instance.IsSignedIn)
           {
               AuthenticationService.Instance.SignOut();
           }
           AuthenticationService.Instance.SwitchProfile(ProfileName);
           await AuthenticationService.Instance.SignInAnonymouslyAsync();

           var options = new SessionOptions()
           {
               Name = SessionName,
               MaxPlayers = _maxPlayers
           }.WithDistributedAuthorityNetwork();

           _session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(SessionName, options);

           _state = ConnectionState.Connected;
       }
       catch (Exception e)
       {
           _state = ConnectionState.Disconnected;
           Debug.LogException(e);
       }
   }
}