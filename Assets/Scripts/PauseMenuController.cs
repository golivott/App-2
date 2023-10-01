using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    private UIDocument doc;
    private Button restartButton;
    private Button exitButton;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        restartButton = doc.rootVisualElement.Q<Button>("RestartBtn");
        exitButton = doc.rootVisualElement.Q<Button>("ExitBtn");
        
        restartButton.clicked += RestartBtnClicked;
        exitButton.clicked += ExitBtnClicked;
    }

    void RestartBtnClicked()
    {
        SceneManager.LoadScene("Level 1");
    }
    void ExitBtnClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}
