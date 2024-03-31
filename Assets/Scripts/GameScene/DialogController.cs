using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
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
        // DOTween�� ����Ͽ� TextMeshPro�� ���ڸ� ����ϴ� �ִϸ��̼��� �����ݴϴ�.
        text.DOText(message, 1f)
            .OnComplete(() => OnTextAnimationComplete()); // �ִϸ��̼��� ����� �� ȣ���� �ݹ� �޼��带 �����մϴ�.    
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

    // �ؽ�Ʈ �ִϸ��̼��� ����� �� ȣ��Ǵ� �ݹ� �޼���
    private void OnTextAnimationComplete()
    {
        buttonNext.gameObject.SetActive(true);
        iconNext.SetActive(true);
    }
}
