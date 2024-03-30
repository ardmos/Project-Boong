using UnityEngine;
using UnityEngine.UI;

public class StaminaUIController : MonoBehaviour
{
    private Slider slider;

    public PlayerStaminaSystem playerStaminaSystem;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        slider.maxValue = Player.DEFAULT_STAMINA_MAX;
        slider.value = playerStaminaSystem.GetStamina();
    }

    public void SetUI(float sliderValue)
    {
        slider.value = sliderValue;
    }
}
