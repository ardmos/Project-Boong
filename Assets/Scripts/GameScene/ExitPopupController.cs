using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPopupController : MonoBehaviour
{
    public Button buttonYes;
    public Button buttonNo;

    private void Start()
    {
        buttonYes.onClick.AddListener(OnGoHome);
        buttonNo.onClick.AddListener(Hide);

        Hide();
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
