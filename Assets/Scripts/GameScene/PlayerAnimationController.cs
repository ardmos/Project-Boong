using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public GameObject vfxJumpPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartJumpAnimation()
    {       
        if(GetComponent<Animator>().GetBool("Jump")) {
            StartCoroutine(StopJumpAnimation());
            return;
        }
        GetComponent<Animator>().SetBool("Jump", true);
    }

    public void ShowJumpVFX()
    {
        // ¿ÃµøΩ√ VFX
        GameObject vfxObj = Instantiate(vfxJumpPrefab, transform);
    }

    private IEnumerator StopJumpAnimation()
    {
        Debug.Log($"StopJumpAnimation(), 1. Jump: {GetComponent<Animator>().GetBool("Jump")}");
        GetComponent<Animator>().SetBool("Jump", false);
        yield return new WaitUntil(() => GetComponent<Animator>().GetBool("Jump")==false);
        GetComponent<Animator>().SetBool("Jump", true);
        Debug.Log($"StopJumpAnimation(), 2. Jump: {GetComponent<Animator>().GetBool("Jump")}");
    }
}
