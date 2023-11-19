using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void TranstionScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
