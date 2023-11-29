using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseParent;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseParent.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void TranstionScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
