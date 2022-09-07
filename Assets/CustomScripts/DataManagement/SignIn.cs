using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class SignIn : MonoBehaviour
{

    [SerializeField]
    public TMP_InputField inputUsername;
    [SerializeField]
    public TMP_InputField inputPassword;

    [SerializeField]
    public KatibInteraction katibInteraction;

    /// <summary>
    /// If the log in details are correct, it moves to the next menu. Otherwise, an error message is printed
    /// </summary>
    /// <param name="nextMenu">The index of the next menu</param>
    public void LogIn()
    {

        katibInteraction.Open();


        // Debug.Log(inputUsername.text);
        // Debug.Log(inputPassword.text);
        // string username = inputUsername.text;
        // username = username.ToLower();
        // string password = inputPassword.text;
        // if (Server.Authenticate(SpreadSheets.Learners, username, password))
        // {
        //     Menu menu = this.GetComponent<Menu>();
        //     SceneManager.LoadScene("Main");
        // }
        // else
        // {
        //     Debug.LogError("Incorrect log in details");
        // }
    }
}
