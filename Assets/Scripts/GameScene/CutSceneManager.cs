using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }

    public GameObject cutSceneIntro;
    public GameObject cutSceneTimeout;
    public GameObject cutSceneCaughtByPuppy;
    public GameObject cutSceneWin;

    private void Awake()
    {
        Instance = this;

        cutSceneIntro.SetActive(false);
        cutSceneTimeout.SetActive(false);
        cutSceneCaughtByPuppy.SetActive(false);
        cutSceneWin.SetActive(false);
    }

    private void Start()
    {
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

    public void ShowIntro()
    {
        // ������� ��Ʈ�� ���� �ƾ� ����. 
        // ����� 
        // 1. �� �ƾ����� ��Ʈ�ѷ��� �ٿ��� �����Ѵ�. 
/*        cutSceneIntro.SetActive(true);

        DialogManager.Instance.ShowDialog("����... �����?", 1f);*/
    }
}
