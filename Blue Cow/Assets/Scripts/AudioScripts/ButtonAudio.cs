using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{

    public AudioSource myAudio;
    public AudioClip hoverAudio;
    public AudioClip ClickAudio;

    public void HoverSond()
    {
        myAudio.PlayOneShot(hoverAudio);
    }
    public void ClickSound()
    {
        myAudio.PlayOneShot(ClickAudio);
    }

}
