using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartJumpAnimation()
    {       
        if(animator.GetBool("Jump")) {
            StartCoroutine(StopJumpAnimation());
            return;
        }
        animator.SetBool("Jump", true);
    }

    private IEnumerator StopJumpAnimation()
    {
        Debug.Log($"StopJumpAnimation(), 1. Jump: {animator.GetBool("Jump")}");
        animator.SetBool("Jump", false);
        yield return new WaitUntil(() => animator.GetBool("Jump")==false);
        animator.SetBool("Jump", true);
        Debug.Log($"StopJumpAnimation(), 2. Jump: {animator.GetBool("Jump")}");
    }
}
