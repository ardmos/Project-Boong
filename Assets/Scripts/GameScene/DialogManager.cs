using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public Transform dialogParent;
    public GameObject dialogPrefab;

    private void Awake()
    {
        Instance = this;
    }

    // 문자열을 출력하는 메소드
    public void ShowDialog(string message)
    {
        DialogController dialog = Instantiate(dialogPrefab, dialogParent).GetComponent<DialogController>();
        dialog.SetMessage(message);
    }

    public void ShowDialog(string message, float seconds)
    {
        StartCoroutine(WaitAndSetMessage(message, seconds));
    }

    private IEnumerator WaitAndSetMessage(string message, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DialogController dialog = Instantiate(dialogPrefab, dialogParent).GetComponent<DialogController>();
        dialog.SetMessage(message);
    }
}
