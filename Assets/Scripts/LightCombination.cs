using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

public class LightCombination : NetworkBehaviour
{
    [SerializeField] private List<int> answer = new List<int> { 1, 3, 5, 7 };
    private List<int> attempt = new List<int>();

    public void AddAttempt(int id)
    {
        attempt.Add(id);
        for (int i = 0; i < attempt.Count; i++)
        {
            Debug.Log(attempt[i]);
            if (attempt[i] != answer[i])
            {
                attempt.Clear();
                Debug.Log("Wrong answer! Attempt cleared!");
            }
        }
        if (attempt.Count == answer.Count)
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        Debug.Log("The door is now opened!");
    }
}
