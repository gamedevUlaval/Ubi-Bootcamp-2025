using System.Collections;
using Unity.Netcode;
using UnityEngine;
using System.Linq;

public class ObjectsCheck : NetworkBehaviour
{
    public Transform targetPosition;
    public float tolerance = 0.5f;
    [SerializeField] private GameObject statusLight;

    void Update()
    {
        // Debug.Log(IsCorrectlyPlaced());
    }

    public IEnumerator TurnOnStatusLight()
    {
        if (IsCorrectlyPlaced())
        {
            statusLight.SetActive(true);
            statusLight.GetComponent<Light>().color = Color.green;
        }
        else
        {
            statusLight.SetActive(true);
            statusLight.GetComponent<Light>().color = Color.white;
        }

        yield return new WaitForSeconds(3f);
        statusLight.SetActive(false);
    }

    public bool IsCorrectlyPlaced()
    {
        return Vector3.Distance(transform.position, targetPosition.position) < tolerance;
    }
}
