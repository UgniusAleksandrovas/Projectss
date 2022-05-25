using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerSkinLobby : MonoBehaviour
    {
        [SerializeField] Inventory inventory;

        [Header("Skin Settings")]
        [SerializeField] PlayerControllerLobby playerScript;
        int activeSkin = 0;
        [SerializeField] SkinLobby[] allSkins;

        private void Awake()
        {
            inventory = FindObjectOfType<Inventory>();
        }

        void OnEnable()
        {
            Inventory.OnChangeActiveSkin += SetActiveSkin;
        }

        void OnDisable()
        {
            Inventory.OnChangeActiveSkin -= SetActiveSkin;
        }

        void Start()
        {
            SetActiveSkin();
        }

        public void SetActiveSkin()
        {
            activeSkin = inventory.activeSkin;
            for (int i = 0; i < allSkins.Length; i++)
            {
                allSkins[i].skin.SetActive(false);
            }
            allSkins[activeSkin].skin.SetActive(true);
            playerScript.anim = allSkins[activeSkin].anim;
        }
    }

    [System.Serializable]
    public struct SkinLobby
    {
        public string name;
        public GameObject skin;
        public Animator anim;
    }
}
