using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject howPlayGamePanel, startMenuPanel;

    private void Awake()
    {
        startMenuPanel.SetActive(true);
        howPlayGamePanel.SetActive(false);
    }

    public void startButton()
    {
        if(!howPlayGamePanel.activeSelf)
        {
            HowToPlayButton();
            return;
        }

        SceneOperations.NextScene();
    }

    public void HowToPlayButton()
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
