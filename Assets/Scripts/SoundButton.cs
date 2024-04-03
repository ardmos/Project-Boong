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
        if (UIButtonSoundManager.Instance == null) return;

        UIButtonSoundManager.Instance.PlayButtonClickSound(soundButtonType);
    }
}
