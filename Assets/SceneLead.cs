using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLead : MonoBehaviour
{
    public static IEnumerator ResetScene(float delay)
    {
        Scene current = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(current.buildIndex);
    }
}
