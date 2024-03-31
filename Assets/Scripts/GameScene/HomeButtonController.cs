using UnityEngine;
using UnityEngine.UI;

public class HomeButtonController : MonoBehaviour
{
    public Button buttonHome;
    public ExitPopupController exitPopupController;

    private void OnEnable()
    {
        buttonHome.onClick.AddListener(OnShowExitPopup);
    }

    private void OnDisable()
    {
        buttonHome.onClick.RemoveListener(OnShowExitPopup);
    }

    private void OnShowExitPopup()
    {
        exitPopupController.Show();
    }
}
