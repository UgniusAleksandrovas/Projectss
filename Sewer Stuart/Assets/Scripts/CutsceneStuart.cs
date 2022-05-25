using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneStuart : MonoBehaviour
{
    [SerializeField] float idleLookTime = 14f;
    [SerializeField] float idleRunTime = 18f;

    public Animator anim;

    public void Begin()
    {
        StartCoroutine(IdleLook());
        StartCoroutine(IdleRun());
    }

    IEnumerator IdleLook()
    {
        yield return new WaitForSeconds(idleLookTime);
        anim.SetBool("Look", true);
    }

    IEnumerator IdleRun()
    {
        yield return new WaitForSeconds(idleRunTime);
        anim.SetBool("Ready", true);
    }
}
