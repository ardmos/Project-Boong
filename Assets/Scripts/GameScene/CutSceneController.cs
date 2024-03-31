using System.Collections;
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

    private void OnEnable()
    {
        buttonRestart.gameObject.SetActive(false);
        buttonGiveUp.gameObject.SetActive(false);
        buttonHome.gameObject.SetActive(false);
        buttonRestart.onClick.AddListener(OnRestartGame);
        buttonGiveUp.onClick.AddListener(OnGoHome);
        buttonHome.onClick.AddListener(OnGoHome);
    }

    private void OnDisable()
    {
        buttonRestart.onClick.RemoveListener(OnRestartGame);
        buttonGiveUp.onClick.RemoveListener(OnGoHome);
        buttonHome.onClick.RemoveListener(OnGoHome);
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

    private void OnRestartGame()
    {
        buttonRestart.gameObject.SetActive(false);
        buttonGiveUp.gameObject.SetActive(false);
        buttonHome.gameObject.SetActive(false);
        gameObject.SetActive(false);
        // 게임 재시작. 
        GameManager.Instance.SetGameState(GameState.Intro);
    }

    private void OnGoHome()
    {
        SceneManager.LoadScene("TitleScene");
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
