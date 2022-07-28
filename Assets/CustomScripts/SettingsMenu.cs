using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// Upon logging out, all cached data is wiped out and user is directed back to the log in scene
    /// </summary>
    public void pressedLogOut()
    {
        List<string> menuKeys = new List<string>();
        foreach (KeyValuePair<string, Stack<string>> entry in Data.lastActiveSceneMenus)
        {
            menuKeys.Add(entry.Key);
        }
        foreach (string key in menuKeys)
        {
            Data.lastActiveSceneMenus[key] = new Stack<string>();
        }
        SceneManager.LoadScene("LogIn");
    }
}
