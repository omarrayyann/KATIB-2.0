using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;


public class RegisterScript : MonoBehaviour
{
    /// <summary>
    /// Text that displays error messages to the user
    /// </summary>
    [SerializeField]
    private TMP_Text error = null;

    /// <summary>
    /// Checks if entered info describes a valid user. Displays the error based on the invalid fields.
    /// </summary>
    /// <param name="fields"></param>
    /// <returns>True if the information is valid and false otherwise</returns>
    public bool Validate(TMP_InputField[] fields)
    {
        foreach (TMP_InputField field in fields)
        {
            if (field.text.Length == 0) // All fields must have some input
            {
                error.gameObject.SetActive(true);
                error.text = "Make sure required fields aren't empty";
                return false;
            }
        }
        if (!fields[0].text.Contains('@'))
        {
            error.gameObject.SetActive(true);
            error.text = "Make sure the email is in the correct format";
            return false;
        }
        if (!fields[2].text.Equals(fields[3].text)) // Password and confirm password fields must match
        {
            error.gameObject.SetActive(true);
            error.text = "Passwords must match";
            return false;
        }
        /*if (!Directory.Exists(Application.dataPath + "/Server/Instructors.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Classes.csv");
        }
        else
        {*/
            string[] users = Server.ReadAllStringsFrom(SpreadSheets.Instructors);
                    for (int i = 0; i < users.Length; i++)
                    {
                        string[] entry = users[i].Split(',');
                        if (entry[1].Equals(fields[0].text))
                        {
                            error.gameObject.SetActive(true);
                            error.text = "An account with this email already exists. Try signing in";
                            return false;
                        }
                        if (entry[2].Equals(fields[1].text))
                        {
                            error.gameObject.SetActive(true);
                            error.text = "Username taken";
                            return false;
                        }
                    }
        //}
        
        return true;
    }
     public void pressedRegister()
    {
        TMP_InputField[] fields = GetComponentsInChildren<TMP_InputField>();
        if (Validate(fields)) // If the information is valid, create a user and send to the login scene
        {
            //Generate a UID
            string generatedUID = System.Guid.NewGuid().ToString();
            string newEntry = fields[0].text + "," + fields[1].text + "," + fields[2].text + "," + generatedUID;
            Instructor newInstructor = new Instructor(generatedUID, fields[0].text.ToLower(), fields[1].text.ToLower(), fields[2].text, "", "", new List<string>());
            List<string> toAppend = new List<string>();
            Data.instructors.Add(newInstructor);
            Data.currentInstructor = newInstructor;
            SceneManager.LoadScene("FinishRegistering");
        }
    }

    /// <summary>
    /// Takes the user back to the sign in scene if the signin button is pressed
    /// </summary>
    public void pressedSignIn()
    {
        SceneManager.LoadScene("Login");
    }
}
