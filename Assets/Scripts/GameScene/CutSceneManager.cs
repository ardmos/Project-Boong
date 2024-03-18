using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject cutSceneTimeout;
    public GameObject cutSceneCaughtByPuppy;
    public GameObject cutSceneWin;

    private void Start()
    {
        cutSceneTimeout.SetActive(false);
        cutSceneCaughtByPuppy.SetActive(false);
        cutSceneWin.SetActive(false);

        GameManager.Instance.OnGameOverTimeout += OnGameOverTimeout;
        GameManager.Instance.OnGameOverByPuppy += OnGameOverByPuppy;
        GameManager.Instance.OnGameWin += OnGameWin;
    }

    private void OnGameWin(object sender, System.EventArgs e)
    {
        cutSceneWin.SetActive(true);
    }

    private void OnGameOverByPuppy(object sender, System.EventArgs e)
    {
        cutSceneCaughtByPuppy.SetActive(true);
    }

    private void OnGameOverTimeout(object sender, System.EventArgs e)
    {
        cutSceneTimeout.SetActive(true);
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("OnDisable(): GameManager.Instance is null");
            return;
        }

        GameManager.Instance.OnGameOverTimeout -= OnGameOverTimeout;
        GameManager.Instance.OnGameOverByPuppy -= OnGameOverByPuppy;
        GameManager.Instance.OnGameWin -= OnGameWin;
    }
}
