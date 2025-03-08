using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room : MonoBehaviour
{
    [SerializeField] private Transform cameraTargetPosition;
    public Transform CameraTargetPosition => cameraTargetPosition;
    
    [SerializeField] private string sceneName;
    public string SceneName => sceneName;
    [SerializeField] private string[] adjacentRooms;
    public string[] AdjacentRooms => adjacentRooms;
}