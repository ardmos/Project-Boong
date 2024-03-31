using UnityEngine;
using UnityEngine.UI;

public class StaminaUIController : MonoBehaviour
{
    public PlayerStaminaSystem playerStaminaSystem;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = Player.DEFAULT_STAMINA_MAX;
        slider.value = playerStaminaSystem.GetStamina();
    }

    public void SetUI(float sliderValue)
    {
        slider.value = sliderValue;
    }
}
