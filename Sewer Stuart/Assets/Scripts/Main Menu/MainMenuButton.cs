using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [HideInInspector] public bool canUse = false;

    [Header("Button Settings")]
    public Transform playerHopPosition;
    public bool shouldPlayerHop;
    [SerializeField] AudioSource sfx;

    [Header("UI Settings")]
    [SerializeField] Text text;
    [SerializeField] Image image;
    [SerializeField] Color enabledColor;
    [SerializeField] Color disabledColor;

    [Header("Light Settings")]
    [SerializeField] Light spotLight;
    [SerializeField] float enabledIntensity = 10f;
    [SerializeField] float disabledIntensity = 2f;
    [SerializeField] GameObject volumetricLightShaft;

    PlayerHop playerAnimation;
    [HideInInspector] public MainMenuButtonManager manager;

    void Awake()
    {
        manager = FindObjectOfType<MainMenuButtonManager>();
        playerAnimation = FindObjectOfType<PlayerHop>();
    }

    void OnMouseEnter()
    {
        if (canUse)
        {
            manager.SelectButton(this);
        }
    }

    void OnMouseExit()
    {
        if (canUse)
        {
            manager.DeselectAllButtons();
        }
    }

    public void SelectButton()
    {
        image.color = enabledColor;
        text.color = enabledColor;
        spotLight.intensity = enabledIntensity;
        volumetricLightShaft.SetActive(true);
        sfx.Play();
        if (playerHopPosition != null)
        {
            playerAnimation.SetTargetPosition(playerHopPosition.position);
        }
    }

    public void DeselectButton()
    {
        image.color = disabledColor;
        text.color = disabledColor;
        spotLight.intensity = disabledIntensity;
        volumetricLightShaft.SetActive(false);
    }
}
