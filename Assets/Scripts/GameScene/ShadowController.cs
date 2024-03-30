using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public void EnableShadow()
    {
        gameObject.SetActive(true);
    }
    public void DisableShadow()
    {
        gameObject.SetActive(false);
    }
}
