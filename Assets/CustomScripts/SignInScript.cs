using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.Android;

public class SignInScript : MonoBehaviour
{
    /// <summary>
    /// List of instructors. Currently the data is received manually, but AWS databases will provide the information later
    /// </summary>

    [SerializeField]
    private GameObject navigation = null;

    [SerializeField]
    private TMP_Text error = null;

    private bool isUpdating = false;
    /*private void Update()
    {
       *//* if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }*//*
    }*/
    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }

    void Start()
    {
        Debug.Log("Started");
        Data.currentLearner = new Learner("", "", "", "", "", "", System.DateTime.Now, "", new List<string>());
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("I've got permissions");
        }
        else
        {
            Debug.Log("I'm asking for permission");
            bool useCallbacks = false;
            if (!useCallbacks)
            {
                // We do not have permission to use the microphone.
                // Ask for permission or proceed without the functionality enabled.
                Permission.RequestUserPermission(Permission.Microphone);
            }
            else
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(Permission.Microphone, callbacks);
            }
        }
    }

    public void PermissionGranted()
    {
        Debug.Log("Granted");
    }

    public void PermissionDenied()
    {
        Debug.Log("Denied");
    }

    /// Logs the user in if the log in button is pressed and the input is correct
    /// </summary>
    public void pressedLogIn()
    {
        // Request();
        string errorMessage = "";
        error.text = errorMessage;
        TMP_InputField[] fields = GetComponentsInChildren<TMP_InputField>();
        string username = fields[0].text.ToLower();
        string password = fields[1].text;
        if (Server.Authenticate(SpreadSheets.Instructors, username, password))
        {
            SceneManager.LoadScene("LearnersTab");
        }
        else
        {
           error.text ="Incorrect username or password";
        }
    }

    /// <summary>
    /// Requesting necessary permissions for the app to run on the current device
    /// </summary>
    /*private void Request()
    {
        //StartCoroutine(GetLocation());
        if (*//*!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||*//* !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            bool useCallbacks = false;
            if (!useCallbacks)
            {
                // We do not have permission to use the microphone.
                // Ask for permission or proceed without the functionality enabled.
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                // Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
            else
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                // Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
        }
    }*/

    /// <summary>
    /// Takes the user to the registration scene once the Register button is pressed
    /// </summary>
    public void pressedRegister(){
        // Request();
        SceneManager.LoadScene("Register");
    }
}
