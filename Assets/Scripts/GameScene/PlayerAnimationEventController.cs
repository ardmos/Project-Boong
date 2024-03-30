using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    private PlayerVFXController playerVFXController;

    private void Awake()
    {
        playerVFXController = GetComponentInParent<PlayerVFXController>();
    }

    public void StartJumpVFX()
    {
        playerVFXController.StartJumpVFX();
    }
}
