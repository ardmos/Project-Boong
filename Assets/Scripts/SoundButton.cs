using UnityEngine.UI;

public class SoundButton : Button
{
    public SoundButtonType soundButtonType;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(PlayClickSound); // 클릭 이벤트에 소리 재생 메서드를 추가
    }

    private void PlayClickSound()
    {
        if (UIButtonSoundManager.Instance == null) return;

        UIButtonSoundManager.Instance.PlayButtonClickSound(soundButtonType);
    }
}
