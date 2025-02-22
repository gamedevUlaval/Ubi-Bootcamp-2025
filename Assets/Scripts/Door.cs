using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] bool isLocked;
    public bool IsLocked => isLocked;
    
    public void Open()
    {
        if (!isLocked)
        {
            StartCoroutine(openDoor());
        }
    }
    
    public void Unlock()
    {
        isLocked = false;
    }
    
    IEnumerator openDoor()
    {
        transform.DOMoveZ(transform.position.z - 1f, 1f);
        yield return new WaitForSeconds(1f);
        transform.DOMoveZ(transform.position.z + 1f, 1f);
    }
}
