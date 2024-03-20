using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button buttonNext;
    public GameObject iconNext;

    public void SetCallback(UnityAction callback)
    {
        UnityAction newCallback = () =>
        {
            callback?.Invoke();
            Destroy(gameObject);
        };

        buttonNext.onClick.AddListener(newCallback);
    }

    public void SetMessage(string message)
    {
        // DOTween�� ����Ͽ� TextMeshPro�� ���ڸ� ����մϴ�
        text.DOText(message, 1f)
            .OnComplete(() => OnTextAnimationComplete()); // �ִϸ��̼��� ����� �� ȣ���� �ݹ� �޼��带 �����մϴ�.    
        buttonNext.gameObject.SetActive(false);
        iconNext.SetActive(false);
    }

    // �ؽ�Ʈ �ִϸ��̼��� ����� �� ȣ��Ǵ� �ݹ� �޼���
    private void OnTextAnimationComplete()
    {
        buttonNext.gameObject.SetActive(true);
        iconNext.SetActive(true);
    }
}
