using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CutSceneType
{
    Intro,
    GameOverTimeout,
    GameOverByPuppy,
    Win
}

public class CutSceneController : MonoBehaviour
{
    public CutSceneType type;
    public Button buttonStart;
    public Button buttonRestart;
    public Button buttonGiveUp;
    public Button buttonHome;

    private void Start()
    {
        buttonStart.gameObject.SetActive(false);
        buttonRestart.gameObject.SetActive(false);
        buttonGiveUp.gameObject.SetActive(false);
        buttonHome.gameObject.SetActive(false);
        buttonStart.onClick.AddListener(OnStartGame);
        buttonRestart.onClick.AddListener(OnRestartGame);
        buttonGiveUp.onClick.AddListener(OnGoHome);
        buttonHome.onClick.AddListener(OnGoHome);
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }

    private void OnRestartGame()
    {
        gameObject.SetActive(false);
    }

    private void OnGoHome()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ShowCutScene()
    {
        gameObject.SetActive(true);
    }

    public void HideCutScene()
    {
        gameObject.SetActive(false);
    }

    public void ActivateButtons()
    {
        StartCoroutine(ActivateButtonWithAnimation());
    }

    private IEnumerator ActivateButtonWithAnimation()
    {
        switch (type)
        {
            case CutSceneType.Intro:
                //buttonStart.gameObject.SetActive(true);    
                break;
            case CutSceneType.GameOverTimeout:
                yield return new WaitForSeconds(1f);
                buttonRestart.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                buttonGiveUp.gameObject.SetActive(true);
                break;
            case CutSceneType.GameOverByPuppy:
                yield return new WaitForSeconds(1f);
                buttonRestart.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                buttonGiveUp.gameObject.SetActive(true);
                break;
            case CutSceneType.Win:
                yield return new WaitForSeconds(1f);
                buttonHome.gameObject.SetActive(true);
                break;
        }
    }
}
