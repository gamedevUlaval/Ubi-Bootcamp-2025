using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class LightLamps : NetworkBehaviour
{
    [SerializeField] private float delay = 10.0f;
    [SerializeField] private GameObject[] lamps;

    private void Start()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        for (int i = 0; i < lamps.Length; i++)
        {
            Renderer _renderer = lamps[i].GetComponent<Renderer>();
            Color defaultcolor = _renderer.material.color;
            _renderer.material.SetColor("_BaseColor", Color.yellow);
            yield return new WaitForSeconds(delay);
            _renderer.material.SetColor("_BaseColor", defaultcolor);
        }
    }
}
