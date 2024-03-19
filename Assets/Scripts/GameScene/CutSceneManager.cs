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
        // 여기부터 인트로 포함 컷씬 구현. 
        // 예상안 
        // 1. 각 컷씬마다 컨트롤러를 붙여서 구현한다. 
/*        cutSceneIntro.SetActive(true);

        DialogManager.Instance.ShowDialog("여긴... 어디지?", 1f);*/
    }
}
