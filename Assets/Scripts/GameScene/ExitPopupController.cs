using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPopupController : MonoBehaviour
{
    public Button buttonYes;
    public Button buttonNo;

    private void OnEnable()
    {
        buttonYes.onClick.AddListener(OnGoHome);
        buttonNo.onClick.AddListener(Hide);
    }

    private void Start()
    {
        Hide();
    }

    private void OnDisable()
    {
        buttonYes.onClick.RemoveListener(OnGoHome);
        buttonNo.onClick.RemoveListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnGoHome()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
