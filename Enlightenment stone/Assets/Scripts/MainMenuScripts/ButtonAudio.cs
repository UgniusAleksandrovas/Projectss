using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{

    public AudioSource myAudio;
    public AudioClip hoverAudio;
    public AudioClip clickAudio;


    public void HoverSound()
    {
        myAudio.PlayOneShot(hoverAudio);
    }

    public void ClickSouns()
    {
        myAudio.PlayOneShot(clickAudio);
    }
}
