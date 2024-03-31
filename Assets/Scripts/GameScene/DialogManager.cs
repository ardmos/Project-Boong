using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public Transform dialogParent;
    public GameObject dialogPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// 다이얼로그를 실행시키는 메서드 입니다. 다이얼로그가 닫힐 때 실행시킬 콜백 함수를 지정해줄 수 있습니다.
    /// 실행시킬 콜백 함수가 없는 경우, callBack 파라미터 값을 null로 설정해주면 됩니다. 
    /// </summary>
    public void ShowDialog(string message, UnityAction callBack)
    {
        DialogController dialog = Instantiate(dialogPrefab, dialogParent).GetComponent<DialogController>();
        dialog.SetCallback(callBack);
        dialog.SetMessage(message);
    }
}
