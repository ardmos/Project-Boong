using System;
using UnityEngine;

public enum SoundButtonType
{
    Normal,
    Start
}

public class UIButtonSoundManager : MonoBehaviour
{
    public AudioClip[] buttonSoundClips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonClickSound(SoundButtonType soundButtonType)
    {
        if (audioSource == null) return;
        if (buttonSoundClips.Length < Enum.GetValues(typeof(SoundButtonType)).Length) return;

        audioSource.clip = buttonSoundClips[(int)soundButtonType];
        audioSource.Play();
    }
}
