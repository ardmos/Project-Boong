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
    public Button buttonRestart;
    public Button buttonGiveUp;
    public Button buttonHome;

    private void Start()
    {
        buttonRestart.gameObject.SetActive(false);
        buttonGiveUp.gameObject.SetActive(false);
        buttonHome.gameObject.SetActive(false);
        buttonRestart.onClick.AddListener(OnRestartGame);
        buttonGiveUp.onClick.AddListener(OnGoHome);
        buttonHome.onClick.AddListener(OnGoHome);
    }

    private void OnRestartGame()
    {
        gameObject.SetActive(false);
        // 게임 재시작. 
        // 1. Game State
        // 2. CutScene State
        // 3. Puppy State 
        // ...!
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
