using System.Collections;
using UnityEngine;

public class DialogSoundManager : MonoBehaviour
{
    public static DialogSoundManager Instance {  get; private set; }

    public AudioClip typingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void PlayTypingSound()
    {
        if (typingSound == null) return;
        if (audioSource == null) return;    

        audioSource.PlayOneShot(typingSound);
    }
}
