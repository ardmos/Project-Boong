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
        Debug.Log($"2.playerData.stamina: {playerData.stamina}");
        if (playerData.stamina < Player.DEFAULT_STAMINA_CONSUMPTION) playerData.stamina = 0f;
        else playerData.stamina -= Player.DEFAULT_STAMINA_CONSUMPTION;
        Debug.Log($"3.playerData.stamina: {playerData.stamina}");
        staminaUIController.SetUI(playerData.stamina);
    }
    private void RecoverStamina(PlayerData playerData)
    {
        if (playerData.stamina + Player.DEFAULT_STAMINA_RECOVERY_AMOUNT > Player.DEFAULT_STAMINA_MAX)
        {
            playerData.stamina = Player.DEFAULT_STAMINA_MAX;
            // ���¹̳� ȸ�� ����
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
