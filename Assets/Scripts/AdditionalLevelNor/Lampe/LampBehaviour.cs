 using UnityEngine;
using Unity.Netcode;
using PlayerControls;
using System.Collections;

public class LampBehaviour : NetworkBehaviour
{
    [SerializeField] private GameObject lampLight;
    [SerializeField] private float timeBeforeOffLampLight;
    [SerializeField] private PlayerInputHandler playerControls;
    [SerializeField] private bool isPlayerNear;


    void Update()
    {   
        if (isPlayerNear)
        {
            if (playerControls.InteractInput)
            {
                //Call Puzzle Validator 
                TurnOnLampRpc();
                
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ghost"))
        {
            playerControls = other.gameObject.GetComponent<PlayerInputHandler>();   
            isPlayerNear = true;
        }   
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ghost"))
        {   
            isPlayerNear = false;
        }   
    }
    IEnumerator LightOnCoolDown()
    {
        lampLight.SetActive(true);
        yield return new WaitForSeconds(timeBeforeOffLampLight);
        lampLight.SetActive(false);
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    void TurnOnLampRpc()
    {
        StartCoroutine(LightOnCoolDown());
        PuzzleValidator.Instance.CheckIfSolved();
    }
}
