using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeButtonController : MonoBehaviour
{
    public Button buttonHome;
    public ExitPopupController exitPopupController;

    private void Start()
    {
        buttonHome.onClick.AddListener(OnButtonHomeClicked);
    }

    private void OnButtonHomeClicked()
    {
        exitPopupController.Show();
    }
}
