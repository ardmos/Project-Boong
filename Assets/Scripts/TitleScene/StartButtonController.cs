using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    public Button buttonStart;

    private void OnEnable()
    {
        buttonStart.onClick.AddListener(OnStartGame);
    }

    private void OnDisable()
    {
        buttonStart.onClick.RemoveListener(OnStartGame);
    }

    private void OnStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
