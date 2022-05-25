using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MainMenuButton, IMenuButton
{
    [Header("Scene Settings")]
    [SerializeField] float loadSceneDelay = 2f;
    [SerializeField] string sceneName;

    public void OnButtonUse()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(loadSceneDelay);
        FindObjectOfType<Inventory>().SaveData();
        SceneManager.LoadScene(sceneName);
    }
}
