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
        // DOTween을 사용하여 TextMeshPro에 글자를 출력합니다
        text.DOText(message, 1f)
            .OnComplete(() => OnTextAnimationComplete()); // 애니메이션이 종료될 때 호출할 콜백 메서드를 설정합니다.    
        buttonNext.gameObject.SetActive(false);
        iconNext.SetActive(false);
    }

    // 텍스트 애니메이션이 종료될 때 호출되는 콜백 메서드
    private void OnTextAnimationComplete()
    {
        buttonNext.gameObject.SetActive(true);
        iconNext.SetActive(true);
    }
}
