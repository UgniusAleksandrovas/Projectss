using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    OnEnter,
    OnExit
}

public enum DisplayType
{
    SetEnabled,
    SetDisabled,
    SetFlipped
}

[RequireComponent(typeof(Collider))]
public class NightVisionTrigger : MonoBehaviour
{
    [SerializeField] TriggerType triggerType;
    [SerializeField] DisplayType displayType;
    [SerializeField] AudioSource sfx;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.OnEnter)
        {
            RatController player = other.GetComponent<RatController>();
            if (player != null)
            {
                bool choice = true;
                if (displayType == DisplayType.SetDisabled)
                {
                    choice = false;
                }
                if (displayType == DisplayType.SetFlipped)
                {
                    choice = !player.GetNightVisionStatus();
                }
                player.EnableNightVision(choice);
                sfx.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerType == TriggerType.OnExit)
        {
            RatController player = other.GetComponent<RatController>();
            if (player != null)
            {
                bool choice = true;
                if (displayType == DisplayType.SetDisabled)
                {
                    choice = false;
                }
                if (displayType == DisplayType.SetFlipped)
                {
                    choice = !player.GetNightVisionStatus();
                }
                player.EnableNightVision(choice);
                sfx.Play();
            }
        }
    }
}
