using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument doc;
    private Button playButton;
    private Button leaderboardButton;
    private Button exitButton;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        playButton = doc.rootVisualElement.Q<Button>("PlayBtn");
        leaderboardButton = doc.rootVisualElement.Q<Button>("LeaderboardBtn");
        exitButton = doc.rootVisualElement.Q<Button>("ExitBtn");
        
        playButton.clicked += PlayBtnClicked;
        leaderboardButton.clicked += LeaderboardBtnClicked;
        exitButton.clicked += ExitBtnClicked;
    }

    void PlayBtnClicked()
    {
        SceneManager.LoadScene("Level 1");
    }

    void LeaderboardBtnClicked()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    void ExitBtnClicked()
    {
        Application.Quit();
    }
}
