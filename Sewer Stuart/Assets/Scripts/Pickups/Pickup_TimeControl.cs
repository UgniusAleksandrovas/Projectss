using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_TimeControl : Pickup
{
    [Header("Time Settings")]
    [SerializeField] float timeMultiplier = 0.5f;
    [SerializeField] float duration = 5f;

    TimeManager timeManager;

    public override void Initialize()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    public override void PickupItem(RatController player)
    {
        float newTimeScale = timeManager.timeScale * timeMultiplier;
        timeManager.ChangeTimeScaleForDuration(newTimeScale, duration);
    }
}
