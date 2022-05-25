using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
    bool canUseButtons;

    [SerializeField] MainMenuButton[] buttons;
    [SerializeField] int selectedButton;

    PlayerHop playerAnimation;

    void Awake()
    {
        playerAnimation = FindObjectOfType<PlayerHop>();
    }

    void Start()
    {
        canUseButtons = true;
        SetAllButtonsUsable(true);
        SelectButton(selectedButton);
    }

    void Update()
    {
        if (canUseButtons)
        {
            if (selectedButton >= 0)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    if (buttons[selectedButton].shouldPlayerHop)
                    {
                        playerAnimation.StartJump();
                    }
                    buttons[selectedButton].GetComponent<IMenuButton>().OnButtonUse();
                    SetAllButtonsUsable(false);
                    canUseButtons = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedButton > 0)
                {
                    selectedButton--;
                }
                else
                {
                    selectedButton = buttons.Length - 1;
                }
                SelectButton(selectedButton);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedButton < buttons.Length - 1)
                {
                    selectedButton++;
                }
                else
                {
                    selectedButton = 0;
                }
                SelectButton(selectedButton);
            }
        }
    }

    public void SelectButton(MainMenuButton button)
    {
        DeselectAllButtons();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button)
            {
                selectedButton = i;
                buttons[i].SelectButton();
                break;
            }
        }
    }

    public void SelectButton(int button)
    {
        DeselectAllButtons();
        selectedButton = button;
        buttons[button].SelectButton();
    }

    public void DeselectAllButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            selectedButton = -1;
            buttons[i].DeselectButton();
        }
    }

    public void SetAllButtonsUsable(bool canUse)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].canUse = canUse;
        }
        canUseButtons = canUse;
    }
}
