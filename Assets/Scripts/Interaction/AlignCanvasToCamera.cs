using UnityEngine;

[ExecuteInEditMode]
public class AlignCanvasToCamera : MonoBehaviour
{
    void Start()
    {
        var mainCam = Camera.main;
        if (mainCam is not null)
        {
            transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
                mainCam.transform.rotation * Vector3.up);
        }
        
        this.enabled = false;
    }
}