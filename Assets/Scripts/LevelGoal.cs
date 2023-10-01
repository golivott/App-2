using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelGoal : MonoBehaviour
{
    public UIDocument levelWin;
    private Label timeLbl;
    private TextField nameTxt;
    private Button submitBtn;

    private float time;

    private void Awake()
    {
        timeLbl = levelWin.rootVisualElement.Q<Label>("TimeLbl");
        nameTxt = levelWin.rootVisualElement.Q<TextField>("NameTxt");
        submitBtn = levelWin.rootVisualElement.Q<Button>("SubmitBtn");

        submitBtn.clicked += OnSubmitClicked;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            time = col.GetComponent<PlayerController>().time;
            timeLbl.text = time.ToString();
            levelWin.enabled = true;
        }
    }

    void OnSubmitClicked()
    {
        if (nameTxt.text.Length > 0)
        {
            // Submit
            Time.timeScale = 1;
            Leaderboard.Record(nameTxt.text, Mathf.FloorToInt(time * 100000));
            SceneManager.LoadScene("Menu");
        }
    }
}
