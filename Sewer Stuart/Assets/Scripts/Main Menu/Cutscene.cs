using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [SerializeField] float duration = 10f;
    float currentTime;
    [SerializeField] string sceneName;

    [SerializeField] GameObject skipButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!skipButton.activeSelf)
            {
                skipButton.SetActive(true);
            }
            else
            {
                LoadScene(sceneName);
            }
        }

        if (currentTime >= duration)
        {
            LoadScene(sceneName);
        }
        currentTime += Time.deltaTime;
    }

    void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
