using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TimerController timerController;
    public StaminaUIController staminaController;
    public GameObject gamePadObject;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        HideAllUI();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void HideAllUI()
    {
        timerController.gameObject.SetActive(false);
        staminaController.gameObject.SetActive(false);
        gamePadObject.SetActive(false);
    }

    public void ShowAllUI()
    {
        timerController.gameObject.SetActive(true);
        staminaController.gameObject.SetActive(true);
        gamePadObject.SetActive(true);
    }
}
