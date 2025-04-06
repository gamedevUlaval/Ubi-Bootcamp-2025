using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : NetworkBehaviour
{
    public UnityEvent sequenceLampes;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V Pressed");
            sequenceLampes.Invoke();
        };
    }
}
