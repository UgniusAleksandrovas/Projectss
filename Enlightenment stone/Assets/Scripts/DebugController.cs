using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    string cheatBuffer;
    char currentChar;
    string[] cheats = { "GiveArmor", "GiveHealth", "GiveBossStaff", "GiveStaff" };

    public GameObject Boots;
    public GameObject Jeans;
    public GameObject Top;
    public GameObject Hat;

    public GameObject Potion;

    public GameObject BossStaff;
    public GameObject Staff;
    public GameObject Console;
    bool Cheat = false;

    [SerializeField] Text enteredText;


    void Update()
    {
        if (Input.GetKeyDown("`"))
        {
            Cheat = !Cheat;
            Console.SetActive(Cheat);
            Time.timeScale = Cheat ? 0 : 1;
        }


        if (Cheat)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (enteredText.text == "GiveArmor")
                {
                    GiveArmorFunction();
                }
                else if (enteredText.text == "GiveHealth")
                {
                    GiveHealthFunction();
                }
                else if (enteredText.text == "GiveBossStaff")
                {
                    GiveBossStaffFunction();
                }
                else if (enteredText.text == "GiveStaff")
                {
                    GiveStaffFunction();
                }
            }
        }
    }

    void GiveArmorFunction()
    { 
        Instantiate(Hat, transform.position, Quaternion.identity);
        Instantiate(Top, transform.position, Quaternion.identity);
        Instantiate(Jeans, transform.position, Quaternion.identity);
        Instantiate(Boots, transform.position, Quaternion.identity);
    }
    void GiveHealthFunction()
    {
        Instantiate(Potion, transform.position, Quaternion.identity);
        Instantiate(Potion, transform.position, Quaternion.identity);
        Instantiate(Potion, transform.position, Quaternion.identity);
    }
    void GiveBossStaffFunction()
    {
        Instantiate(BossStaff, transform.position, Quaternion.identity);
    }
    void GiveStaffFunction()
    {
        Instantiate(Staff, transform.position, Quaternion.identity);
    }
}
