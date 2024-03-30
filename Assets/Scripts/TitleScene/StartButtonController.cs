using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    public Button buttonStart;

    private void Start()
    {
        buttonStart.onClick.AddListener(OnStartGame);
    }

    private void OnStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
