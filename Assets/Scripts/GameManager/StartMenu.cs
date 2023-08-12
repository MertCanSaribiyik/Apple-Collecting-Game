using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject howPlayGamePanel, startMenuPanel;

    private void Awake()
    {
        Time.timeScale = 0f;
        startMenuPanel.SetActive(true);
        howPlayGamePanel.SetActive(false);
    }

    public void startButton()
    {
        if(!howPlayGamePanel.activeSelf)
        {
            HowPlayGameButton();
            return;
        }

        Time.timeScale = 1f;
        SceneOperations.NextScene();
    }

    public void HowPlayGameButton()
    {
        startMenuPanel.SetActive(false);
        howPlayGamePanel.SetActive(true);
    }

    public void BackButton()
    {
        startMenuPanel.SetActive(true);
        howPlayGamePanel.SetActive(false);
    }
}
