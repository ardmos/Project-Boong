using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    public GameObject vfxJumpPrefab;

    public void StartJumpVFX()
    {
        GameObject vfxObj = Instantiate(vfxJumpPrefab, transform);
    }
}
