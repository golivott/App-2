using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LeaderboardUI : MonoBehaviour
{
    public UIDocument doc;

    private Button exitBtn;
    private void Start()
    {
        exitBtn = doc.rootVisualElement.Q<Button>("ExitBtn");
        exitBtn.clicked += OnClickExit;
        
        int entriesCount = Leaderboard.EntryCount;
        for (int i = 0; i < entriesCount; i++)
        {
            Leaderboard.ScoreEntry entry = Leaderboard.GetEntry(i);
            GroupBox entryUI = doc.rootVisualElement.Q<GroupBox>("Entry"+i);

            Label name = (Label)entryUI.ElementAt(0);
            Label time = (Label)entryUI.ElementAt(1);
            name.text = entry.name;
            time.text = entry.score != Int32.MaxValue ? (entry.score/100000f).ToString() : "";
        }
    }

    void OnClickExit()
    {
        SceneManager.LoadScene("Menu");
    }
}
