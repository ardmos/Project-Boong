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

    public void ReduceStamina(PlayerData playerData)
    {
        if (playerData.stamina < Player.DEFAULT_STAMINA_CONSUMPTION) playerData.stamina = 0f;
        else playerData.stamina -= Player.DEFAULT_STAMINA_CONSUMPTION;

        staminaUIController.SetUI(GetStamina());
    }
    private void RecoverStamina(PlayerData playerData)
    {
        if (playerData.stamina + Player.DEFAULT_STAMINA_RECOVERY_AMOUNT > Player.DEFAULT_STAMINA_MAX)
        {
            playerData.stamina = Player.DEFAULT_STAMINA_MAX;
            // 스태미너 회복 중지
            StopStaminaRecovery();
        }
        else playerData.stamina += Player.DEFAULT_STAMINA_RECOVERY_AMOUNT;

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
            RecoverStamina(Player.Instance.GetPlayerData());
        }
    }
}
