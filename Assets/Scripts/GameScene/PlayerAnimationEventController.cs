using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    private PlayerVFXController playerVFXController;

    private void Start()
    {
        playerVFXController = GetComponentInParent<PlayerVFXController>();
    }

    public void StartJumpVFX()
    {
        playerVFXController.StartJumpVFX();
    }
}
