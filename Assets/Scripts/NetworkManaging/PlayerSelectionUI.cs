using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
    // Méthode pour sélectionner le rôle Ghost (0)
    public void SelectGhost()
    {
        if (CustomPlayerSpawner.Instance != null)
            CustomPlayerSpawner.Instance.OnSelectRole(0);
    }

    // Méthode pour sélectionner le rôle Human (1)
    public void SelectHuman()
    {
        if (CustomPlayerSpawner.Instance != null)
            CustomPlayerSpawner.Instance.OnSelectRole(1);
    }
}