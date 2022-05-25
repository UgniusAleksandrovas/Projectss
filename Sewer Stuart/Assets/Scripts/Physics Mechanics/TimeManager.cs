using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeScale = 1f;
    float previousTimeScale = 1f;
    [SerializeField] float physicsTargetFPS = 50;
    [SerializeField] GameObject screenEffect;

    void Awake()
    {
        timeScale = 1f;
        ChangeTimeScale(1f);
    }

    public void ChangeTimeScale(float newTimeScale)
    {
        previousTimeScale = timeScale;
        Time.timeScale = newTimeScale;
        if (newTimeScale >= 0)
        {
            Time.fixedDeltaTime = newTimeScale / physicsTargetFPS;
        }
    }

    Coroutine timeScaleChange;
    IEnumerator ChangeTimeScaleForDurationCoroutine(float newTimeScale, float duration)
    {
        previousTimeScale = timeScale;
        Time.timeScale = newTimeScale;
        if (newTimeScale >= 0)
        {
            Time.fixedDeltaTime = newTimeScale / physicsTargetFPS;
        }
        screenEffect.SetActive(true);
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = previousTimeScale;
        if (previousTimeScale >= 0)
        {
            Time.fixedDeltaTime = previousTimeScale / physicsTargetFPS;
        }
        screenEffect.SetActive(false);
    }

    public void ChangeTimeScaleForDuration(float newTimeScale, float duration)
    {
        if (timeScaleChange != null)
        {
            StopCoroutine(timeScaleChange);
        }
        timeScaleChange = StartCoroutine(ChangeTimeScaleForDurationCoroutine(newTimeScale, duration));
    }
}
