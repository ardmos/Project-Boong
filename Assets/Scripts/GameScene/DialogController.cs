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

    // ȿ������ ����ϴ� �޼���
    private void PlayTypingSound()
    {
        DialogSoundManager.Instance?.PlayTypingSound();
    }

    // �ؽ�Ʈ �ִϸ��̼��� ����� �� ȣ��Ǵ� �ݹ� �޼���
    private void OnTextAnimationComplete()
    {
        buttonNext.gameObject.SetActive(true);
        iconNext.SetActive(true);
    }

    // �� ���ھ� ����ִ� �ִϸ��̼��� �����ϴ� �޼���
    private IEnumerator TypeSentence(string message, UnityAction callback)
    {
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;

            // ������ �Ҹ��� ���� �ʽ��ϴ�
            if (letter != System.Convert.ToChar(32)) PlayTypingSound();

            yield return new WaitForSeconds(DEFAULT_TYPING_LETTER_INTERVAL);
        }

        callback?.Invoke();
    }
}
