using UnityEngine;
using Steamworks;

public class SteamInviter : MonoBehaviour
{
   // Appelé par un bouton dans l'interface utilisateur pour inviter un ami
    public void InviteFriend()
    {
        // Lance l'overlay Steam pour inviter un ami
        SteamFriends.ActivateGameOverlayInviteDialog(SteamUser.GetSteamID());
        Debug.Log("Invitation Steam envoyée !");
    }
}
