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
    /// ���̾�α׸� �����Ű�� �޼��� �Դϴ�. ���̾�αװ� ���� �� �����ų �ݹ� �Լ��� �������� �� �ֽ��ϴ�.
    /// �����ų �ݹ� �Լ��� ���� ���, callBack �Ķ���� ���� null�� �������ָ� �˴ϴ�. 
    /// </summary>
    public void ShowDialog(string message, UnityAction callBack)
    {
        DialogController dialog = Instantiate(dialogPrefab, dialogParent).GetComponent<DialogController>();
        dialog.SetCallback(callBack);
        dialog.SetMessage(message);
    }
}
