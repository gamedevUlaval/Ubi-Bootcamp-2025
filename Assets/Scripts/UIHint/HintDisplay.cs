using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HintDisplay : MonoBehaviour
{
    public GameObject hintUI;
    [SerializeField] Animator hintAnim;
    [SerializeField] private TextMeshProUGUI hintTMP;
    [SerializeField] private string whoHint; 
    [SerializeField] private float delayBeforeFadeOut;
    [SerializeField, TextArea] private string hintText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(whoHint))// Selon le joueur
        {
            NetworkObject networkObject = other.gameObject.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.IsOwner)
            {
                hintTMP.text = hintText;
                StartCoroutine(HintAnimation());
            }
        }
            
    }   

    IEnumerator HintAnimation()
    {
       hintAnim.SetTrigger("StartAnimation");
       yield return new WaitForSeconds(delayBeforeFadeOut);
       hintAnim.SetTrigger("EndAnimation");
    }
}
