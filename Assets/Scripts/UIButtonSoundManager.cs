using System;
using UnityEngine;

public enum SoundButtonType
{
    Normal,
    Start
}

public class UIButtonSoundManager : MonoBehaviour
{
    public static UIButtonSoundManager Instance { get; private set; }

    public AudioClip[] buttonSoundClips;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClickSound(SoundButtonType soundButtonType)
    {
        if (audioSource == null) return;
        if (buttonSoundClips.Length < Enum.GetValues(typeof(SoundButtonType)).Length) return;

        audioSource.clip = buttonSoundClips[(int)soundButtonType];
        audioSource.Play();
    }
}
