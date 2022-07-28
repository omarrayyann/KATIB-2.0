using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

// Handles the motion between scenes and menus
public class Menu : MonoBehaviour
{
    public MenuGroup menuGroup;
    [SerializeField]
    public bool activeOnStart;


    /*private void Start()
    {
        gameObject.SetActive(activeOnStart);
    }*/

    public void moveTo(int menuNumber)
    {
        menuGroup.menuStack.Push(this);
        menuGroup.menus[menuNumber].gameObject.SetActive(true);
        menuGroup.activeMenu = menuGroup.menus[menuNumber];
        menuGroup.ReturnButtonVisibility();
        if (gameObject.TryGetComponent<SmartText>(out SmartText smart))
        {
            smart.RevertTexts();
        }
        gameObject.SetActive(false);
    }

    public void loadAnother(int menuNumber)
    {
        menuGroup.menuStack.Push(this);
        menuGroup.menus[menuNumber].gameObject.SetActive(true);
        menuGroup.activeMenu = menuGroup.menus[menuNumber];
        menuGroup.ReturnButtonVisibility();
    }

    public void moveTo(Menu otherMenu)
    {
        menuGroup.menuStack.Push(this);
        otherMenu.gameObject.SetActive(true);
        menuGroup.activeMenu = otherMenu;
        menuGroup.ReturnButtonVisibility();
        if (gameObject.TryGetComponent<SmartText>(out SmartText smart))
        {
            smart.RevertTexts();
        }
        gameObject.SetActive(false);
    }

    public void loadAnother(Menu otherMenu)
    {
        menuGroup.menuStack.Push(this);
        otherMenu.gameObject.SetActive(true);
        menuGroup.activeMenu = otherMenu;
        menuGroup.ReturnButtonVisibility();
    }
}
