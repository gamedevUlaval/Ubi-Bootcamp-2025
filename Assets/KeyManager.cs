using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }
    public List<GameObject> keyObjects;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (var key in keyObjects)
        {
            key.SetActive(false);
        }
    }

    public void AddKey(int key)
    {
        if (key >= keyObjects.Count)
        {
            Debug.LogError("Key is out of range.");
            return;
        }
        keyObjects[key].SetActive(true);
    }

    public bool HasKey(int key)
    {
        if (key >= keyObjects.Count)
        {
            Debug.LogError("Key is out of range.");
            return false;
        }
        return keyObjects[key].activeSelf;
    }
}
