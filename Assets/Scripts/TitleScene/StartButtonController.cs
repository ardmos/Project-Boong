using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    private Button buttonStart;

    private void Awake()
    {
        buttonStart = GetComponent<Button>();
    }

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
