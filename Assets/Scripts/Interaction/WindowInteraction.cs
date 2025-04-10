using Unity.Netcode;
using UnityEngine;

public class WindowInteraction : NetworkBehaviour, IInteractable
{
    public Animator lightningAnimation;
    public Vector2 randomnessInterval = new Vector2(1f, 3f);
    public float cooldownTime = 1f;
    private float timer;
    private float cooldown;

    [Header("Sound")]
    [SerializeField] private AudioClip lightningSound;

    void Start()
    {
        timer = randomnessInterval.x;
        cooldown = 0;
    }

    void Update()
    {
        if (!HasAuthority)
            return;
        timer -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (timer <= 0)
        {
            PlayAnimation();
            timer = Random.Range(randomnessInterval.x, randomnessInterval.y);
        }

        if (cooldown <= 0)
        {
            cooldown = 0;
        }
    }
    
    [ContextMenu("Play lightning")]
    public void Interact()
    {
        if (cooldown <= 0)
        {
            PlayAnimation();
            cooldown = cooldownTime;
        }
    }

    private void PlayAnimation()
    {
        lightningAnimation.Play("Lightning");
    }

    public void PlaySound()
    {
        SoundManager.Instance.PlaySFX(lightningSound);
    }

    public bool InteractWith(GameObject tryToInteractWith)
    {
        throw new System.NotImplementedException();
    }

    public InteractableType InteractableType => InteractableType.Cooldown;
}
