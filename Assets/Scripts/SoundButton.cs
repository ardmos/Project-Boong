using UnityEngine.UI;

public class SoundButton : Button
{
    public SoundButtonType soundButtonType;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(PlayClickSound); // Ŭ�� �̺�Ʈ�� �Ҹ� ��� �޼��带 �߰�
    }

    private void PlayClickSound()
    {
        UIButtonSoundManager uISoundManager = FindObjectOfType<UIButtonSoundManager>();
        if (uISoundManager == null) return;

        uISoundManager.PlayButtonClickSound(soundButtonType);
    }
}
