using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject howPlayGamePanel, startMenuPanel;

    [SerializeField]
    private TMPro.TextMeshProUGUI highScore;

    private void Awake()
    {
        startMenuPanel.SetActive(true);
        howPlayGamePanel.SetActive(false);

        highScore.text = "Your Highscore\n" + PlayerPrefs.GetFloat("highScore", 0);
    }

    public void startButton()
    {
        if(!howPlayGamePanel.activeSelf)
        {
            startMenuPanel.SetActive(false);
            howPlayGamePanel.SetActive(true);
            return;
        }

        SceneOperations.NextScene();
    }



    public void BackButton()
    {
        startMenuPanel.SetActive(true);
        howPlayGamePanel.SetActive(false);
    }
}
