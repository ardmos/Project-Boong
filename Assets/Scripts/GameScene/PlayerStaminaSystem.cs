using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    private Coroutine staminaRecoveryCoroutine;

    private float stamina;

    public StaminaUIController staminaUIController;

    private void Awake()
    {
        stamina = Player.DEFAULT_STAMINA_MAX;
    }

    public void ReduceStamina()
    {
        if (stamina < Player.DEFAULT_STAMINA_CONSUMPTION) stamina = 0f;
        else stamina -= Player.DEFAULT_STAMINA_CONSUMPTION;

        Player.Instance.UpdatePlayerStamina(stamina);
        staminaUIController.SetUI(stamina);
    }
    private void RecoverStamina()
    {
        if (stamina + Player.DEFAULT_STAMINA_RECOVERY_AMOUNT > Player.DEFAULT_STAMINA_MAX)
        {
            stamina = Player.DEFAULT_STAMINA_MAX;
            // 스태미너 회복 중지
            StopStaminaRecovery();
        }
        else stamina += Player.DEFAULT_STAMINA_RECOVERY_AMOUNT;

        Player.Instance.UpdatePlayerStamina(stamina);
        staminaUIController.SetUI(GetStamina());
    }

    public void StartStaminaRecovery()
    {
        if (staminaRecoveryCoroutine != null) return;

        staminaRecoveryCoroutine = StartCoroutine(StaminaRecoveryTimer());
    }

    public void StopStaminaRecovery()
    {
        if (staminaRecoveryCoroutine == null) return;

        StopCoroutine(staminaRecoveryCoroutine);
        staminaRecoveryCoroutine = null;
    }

    public float GetStamina()
    {
        return stamina;
    }

    private IEnumerator StaminaRecoveryTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Player.DEFAULT_STAMINA_RECOVERY_INTERVAL);
            RecoverStamina();
        }
    }
}
