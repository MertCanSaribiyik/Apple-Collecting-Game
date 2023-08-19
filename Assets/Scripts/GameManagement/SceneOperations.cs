using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneOperations
{
    public static void NextScene()
    {
        TimeManager.instantiate.elapsedTime = Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void PreviosScene()
    {
        TimeManager.instantiate.elapsedTime = Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void ReloadScene()
    {
        TimeManager.instantiate.elapsedTime = Time.time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
