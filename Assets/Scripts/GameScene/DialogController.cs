using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public const float DEFAULT_TYPING_LETTER_INTERVAL = 0.1f;

    public TextMeshProUGUI text;
    public Button buttonNext;
    public GameObject iconNext;

    private UnityAction callback;

    private void OnDisable()
    {
        buttonNext.onClick.RemoveListener(OnButtonNextClicked);
    }

    public void SetCallback(UnityAction callback)
    {
        this.callback = callback;
        buttonNext.onClick.AddListener(OnButtonNextClicked);
    }

    public void SetMessage(string message)
    {
        StartCoroutine(TypeSentence(message, OnTextAnimationComplete));
        buttonNext.gameObject.SetActive(false);
        iconNext.SetActive(false);
    }

    private void OnButtonNextClicked()
    {
        callback?.Invoke();
        DestroyDialog();
    }

    private void DestroyDialog()
    {
        Destroy(gameObject);
    }

    // 효과음을 재생하는 메서드
    private void PlayTypingSound()
    {
        DialogSoundManager.Instance?.PlayTypingSound();
    }

    // 텍스트 애니메이션이 종료될 때 호출되는 콜백 메서드
    private void OnTextAnimationComplete()
    {
        buttonNext.gameObject.SetActive(true);
        iconNext.SetActive(true);
    }

    // 한 글자씩 찍어주는 애니메이션을 구현하는 메서드
    private IEnumerator TypeSentence(string message, UnityAction callback)
    {
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;

            // 공백은 소리를 내지 않습니다
            if (letter != System.Convert.ToChar(32)) PlayTypingSound();

            yield return new WaitForSeconds(DEFAULT_TYPING_LETTER_INTERVAL);
        }

        callback?.Invoke();
    }
}
