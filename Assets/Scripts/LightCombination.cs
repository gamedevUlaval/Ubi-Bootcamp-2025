using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class LightCombination : NetworkBehaviour
{
    [SerializeField] private List<int> answer = new List<int> { 1, 3, 5, 7 };
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] tiles;
    private List<int> attempt = new List<int>();
    private Color defaultColor;

    public void AddAttempt(int id)
    {
        if (attempt.Count != 0 && id == attempt.Last())
        {
            return;
        }
        Renderer _renderer = tiles[id-1].GetComponent<Renderer>();
        defaultColor = _renderer.material.color;
        
        attempt.Add(id);
        if (attempt.Count <= answer.Count)
            _renderer.material.SetColor("_BaseColor", Color.yellow);

        for (int i = 0; i < attempt.Count; i++)
        {
            if (attempt.Count <= answer.Count && attempt[i] != answer[i])
            {
                attempt.Clear();
                for (int j = 0; j < tiles.Length; j++)
                {
                    Renderer _r = tiles[j].GetComponent<Renderer>();
                    _r.material.SetColor("_BaseColor", defaultColor);
                }
            }
        }
        if (attempt.Count == answer.Count)
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        door.SetActive(false);
    }
}
