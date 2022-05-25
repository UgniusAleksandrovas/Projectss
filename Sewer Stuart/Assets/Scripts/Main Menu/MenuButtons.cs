using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [HideInInspector] public bool canUse = false;
    [SerializeField] PlayAction playAction;

    [Space(20)]
    [SerializeField] ButtonType buttonType = ButtonType.singleplayer;

    void OnMouseEnter()
    {
        if (canUse)
        {
            playAction.SelectButton(buttonType);
        }
    }
}
