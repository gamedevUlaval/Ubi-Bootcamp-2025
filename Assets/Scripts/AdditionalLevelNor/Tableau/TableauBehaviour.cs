using PlayerControls;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
public class TableauBehaviour : MonoBehaviour  /*IInteractable*/
{
    [SerializeField] private GameObject ghostCanvas;
    [SerializeField] private PlayerInputHandler playerControls;
    [SerializeField] private bool isPlayerNear;
    [SerializeField] private float initialCanvasTimer = 20;
    private float currentCanvasTimer = 20;
    [SerializeField] private float timerDeceleration = 0.1f;
    [SerializeField] private bool canvasOn;
    [SerializeField] private bool timerStart;
    [SerializeField] private TextMeshProUGUI compteur;

    [Header("Sound")]
    [SerializeField] private AudioClip clockTicking;

    void Start()
    {
        currentCanvasTimer = initialCanvasTimer;   
    }
    void Update()
    {
        if(isPlayerNear)
        {
            if(playerControls.InteractInput && !canvasOn)
            {   
                SoundManager.Instance.PlayGhostHaunt();
                SoundManager.Instance.PlaySFX(clockTicking);
                ghostCanvas.SetActive(true);
                canvasOn = true;
                timerStart = true;
            }
        }
        if (timerStart)
        {
            StartTimer();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            playerControls = other.gameObject.GetComponent<PlayerInputHandler>();
            isPlayerNear = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            ghostCanvas.SetActive(false);
            isPlayerNear = false;
            canvasOn = false;
        }
    }

    void StartTimer()
    {
        
        compteur.text = currentCanvasTimer.ToString("F0");
        if (currentCanvasTimer > 0)
        {
            currentCanvasTimer -= timerDeceleration;
        }
        else if (currentCanvasTimer <= 0)
        {
            canvasOn = false;   
            timerStart = false;
            ghostCanvas.SetActive(false);
            currentCanvasTimer = initialCanvasTimer;
        }
    }




    ///Interaction avec Systeme d'interaction

    /*public InteractableType InteractableType => InteractableType.Static;

    public void Interact()
    {
        Debug.Log("I am Called");
        ghostCanvas.SetActive(true);
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        return false;
    }*/


}
