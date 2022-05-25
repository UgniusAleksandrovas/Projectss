using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackTrigger : MonoBehaviour
{

    [SerializeField] TriggerType triggerType;
    [SerializeField] DisplayType displayType;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.OnEnter)
        {
            RatController player = other.GetComponent<RatController>();
            if (player != null)
            {
                player.StopSkateboarding();
                player.StopJumpBoost();
                Jetpack jetpack = player.GetComponent<Jetpack>();
                if (jetpack != null)
                {
                    bool choice = true;
                    if (displayType == DisplayType.SetDisabled)
                    {
                        choice = false;
                    }
                    if (displayType == DisplayType.SetFlipped)
                    {
                        choice = !jetpack.enabled;
                    }
                    jetpack.enabled = choice;
                }
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
                Jetpack jetpack = player.GetComponent<Jetpack>();
                if (jetpack != null)
                {
                    bool choice = true;
                    if (displayType == DisplayType.SetDisabled)
                    {
                        choice = false;
                    }
                    if (displayType == DisplayType.SetFlipped)
                    {
                        choice = !jetpack.enabled;
                    }
                    jetpack.enabled = choice;
                }
            }
        }
    }
}
