using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject credits;
    Vector3 creditsStartPosition;
    [SerializeField] float scrollSpeed;
    [SerializeField] Button exitCreditsButton;

    void Awake()
    {
        creditsStartPosition = credits.transform.position;
    }

    private void OnEnable()
    {
        credits.transform.position = creditsStartPosition;
    }

    void Update()
    {
        credits.transform.position += Vector3.up * scrollSpeed * Time.deltaTime;

        if (Input.anyKeyDown)
        {
            if (exitCreditsButton.IsActive())
            {
                CloseCredits();
            }
            else
            {
                exitCreditsButton.gameObject.SetActive(true);
            }
        }
    }

    public void CloseCredits()
    {
        exitCreditsButton.onClick.Invoke();
    }
}
