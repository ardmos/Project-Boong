using UnityEngine;
using UnityEngine.UI;

public class StaminaUIController : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        if (GetComponentInChildren<Slider>() == null)
        {
            Debug.LogError($"{nameof(StaminaUIController)} can't find slider component.");
            return;
        }
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = Player.DEFAULT_STAMINA_MAX;
        slider.value = Player.Instance.GetStamina();
    }

    public void SetUI(float sliderValue)
    {
        slider.value = sliderValue;
    }
}
