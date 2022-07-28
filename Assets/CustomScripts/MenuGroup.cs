using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Holds Menu objects together to facillitate the movement between menus
public class MenuGroup : MonoBehaviour
{
    [SerializeField]
    public Menu[] menus;
    public Menu activeMenu;
    public Stack<Menu> menuStack = new Stack<Menu>();
    [SerializeField]
    private string previousScene = "";
    [SerializeField]
    public bool disableOnClick = false;
    [SerializeField]
    private bool hideReturnOnEmptyStack = false;
    private GameObject returnButton = null;

    // Sets the parent menuGroup of its children menus to itself
    private void Awake()
    {
        if (Data.lastActiveSceneMenus.ContainsKey(SceneManager.GetActiveScene().name))
        {
            string[] menusAsStrings = Data.lastActiveSceneMenus[SceneManager.GetActiveScene().name].ToArray();
            Data.lastActiveSceneMenus[SceneManager.GetActiveScene().name].Clear();
            for (int i = 0; i < menusAsStrings.Length; i++)
            {
                for (int j = 0; j < menus.Length; j++)
                {
                    if (menus[j].gameObject.name.Equals(menusAsStrings[i]))
                    {
                        menuStack.Push(menus[j]);
                        break;
                    }
                }
            }
        }
        if (hideReturnOnEmptyStack)
        {
            returnButton = GameObject.FindGameObjectWithTag("ReturnButton");
            if (menuStack.Count == 0)
                returnButton.SetActive(false);
        }
        foreach (Menu menu in menus)
        {
            menu.menuGroup = this;
            if (menu.activeOnStart && menuStack.Count == 0)
            {
                activeMenu = menu;
                menu.gameObject.SetActive(true);
            }
            else if (menuStack.Count != 0 && menuStack.Peek().Equals(menu))
            {
                activeMenu = menu;
                menu.gameObject.SetActive(true);
            }
            else
            {
                menu.gameObject.SetActive(false);
            }
        }
        if (disableOnClick)
        {
            gameObject.GetComponentsInChildren<Button>()[0].onClick.AddListener(Clicked);
        }
    }

    public void Return()
    {
        if (activeMenu.gameObject.TryGetComponent<SmartText>(out SmartText smart))
        {
            smart.RevertTexts();
        }
        if (menuStack.Count != 0)
        {
            activeMenu.gameObject.SetActive(false);
            activeMenu = menuStack.Pop();
            activeMenu.gameObject.SetActive(true);
        }
        else if (!previousScene.Equals(""))
        {
            LoadPreviousScene();
        }
        ReturnButtonVisibility();
    }

    public void ReturnButtonVisibility()
    {
        if (hideReturnOnEmptyStack)
        {
            if (menuStack.Count == 0)
            {
                returnButton.SetActive(false);
            }
            else
            {
                returnButton.SetActive(true);
            }
        }
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    public void LoadScene(string sceneName)
    {
        if (activeMenu.gameObject.TryGetComponent<SmartText>(out SmartText smart))
        {
            smart.RevertTexts();
        }
        if (sceneName.Equals(SceneManager.GetActiveScene().name))
        {
            ClearStack();
        }
        if (Data.lastActiveSceneMenus.ContainsKey(SceneManager.GetActiveScene().name))
        {
            Menu[] menusInStack = menuStack.ToArray();
            foreach (Menu menu in menusInStack)
            {
                Data.lastActiveSceneMenus[SceneManager.GetActiveScene().name].Push(menu.gameObject.name);
            }
        }
        SceneManager.LoadScene(sceneName);
    }

    public void ClearStack()
    {
        while (menuStack.Count != 0)
        {
            Return();
        }
    }

    public void Clicked()
    {
        if (disableOnClick && menuStack.Count != 0)
            Return();
        else if (menuStack.Count == 0)
            gameObject.SetActive(false);
    }
}
