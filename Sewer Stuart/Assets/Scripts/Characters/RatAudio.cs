using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Vector2 audioPitchRange;
    [SerializeField] AudioClip[] footstep;
    [SerializeField] AudioClip[] skateStep;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip land;
    [SerializeField] AudioClip[] hurt;
    [SerializeField] AudioClip[] success;
    [SerializeField] AudioClip[] pickup;

    public void Footstep()
    {
        audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
        int rand = Random.Range(0, footstep.Length);
        audioSource.PlayOneShot(footstep[rand]);
    }

    public void SkateStep()
    {
        audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
        int rand = Random.Range(0, skateStep.Length);
        audioSource.PlayOneShot(skateStep[rand]);
    }

    public void Jump()
    {
        audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
        audioSource.PlayOneShot(jump);
    }

    public void Land()
    {
        audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
        audioSource.PlayOneShot(land);
    }

    public void Stumble()
    {
        audioSource.pitch = 1f;
        int rand = Random.Range(0, hurt.Length);
        audioSource.PlayOneShot(hurt[rand]);
    }

    public void Successful()
    {
        audioSource.pitch = 1f;
        int rand = Random.Range(0, success.Length);
        audioSource.PlayOneShot(success[rand]);
    }

    public void Pickup()
    {
        audioSource.pitch = Random.Range(audioPitchRange.x, audioPitchRange.y);
        int rand = Random.Range(0, pickup.Length);
        audioSource.PlayOneShot(pickup[rand]);
    }
}
