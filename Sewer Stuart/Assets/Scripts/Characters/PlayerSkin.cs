using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [Header("Skin Settings")]
    [SerializeField] RatController ratController;
    [SerializeField] MagnetPowerUp magnet;
    [SerializeField] Jetpack jetpack;
    int activeSkin = 0;
    [SerializeField] Skin[] allSkins;

    void Start()
    {
        activeSkin = inventory.activeSkin;
        for (int i = 0; i < allSkins.Length; i++)
        {
            allSkins[i].skin.SetActive(false);
        }
        allSkins[activeSkin].skin.SetActive(true);

        ratController.ChangeSkin(
            allSkins[activeSkin].anim,
            allSkins[activeSkin].mesh,
            allSkins[activeSkin].audioController,
            allSkins[activeSkin].springBoots,
            allSkins[activeSkin].iceSkates
        );
        magnet.model = allSkins[activeSkin].magnet;
        jetpack.anim = allSkins[activeSkin].anim;
        jetpack.jetpackModel = allSkins[activeSkin].jetpack;
        jetpack.flames = allSkins[activeSkin].flames;
        jetpack.sfx = allSkins[activeSkin].flamesAudio;
        jetpack.fuelBar = allSkins[activeSkin].jetpackFuelBar;
    }
}

[System.Serializable]
public struct Skin
{
    public string name;
    public GameObject skin;
    public SkinnedMeshRenderer mesh;
    public Animator anim;
    public RatAudio audioController;
    public GameObject magnet;
    public GameObject[] springBoots;
    public GameObject[] iceSkates;
    public GameObject jetpack;
    public VisualEffect[] flames;
    public AudioSource flamesAudio;
    public Image jetpackFuelBar;
}
