using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;


public class FinishRegistering : MonoBehaviour
{
    /// <summary>
    /// Text that displays error messages to the user
    /// </summary>
    [SerializeField]
    private TMP_Text error = null;

    /// <summary>
    /// Error string
    /// </summary>
    private string Error = "";

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
                Error = "Make sure required fields aren't empty";
                error.text = Error;
                return false;
            }
        }
  
        return true;
    }
     public void finishedRegister()
    {   
        TMP_InputField[] fields = GetComponentsInChildren<TMP_InputField>();
        /*if (!Directory.Exists(Application.dataPath + "/Server/Instructors.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Instructors.csv");
        }*/
        if (Validate(fields)) // If the information is valid, create a user and send to the login scene
        {
            for(int i = 0; i<Data.instructors.Count; i++){
                Data.currentInstructor.firstName = fields[0].text;
                Data.currentInstructor.lastName = fields[1].text;

                if (Data.instructors[i].email == Data.currentInstructor.email) {
                    Data.instructors[i] = Data.currentInstructor;
                }
            }
            List<string> toAppend = new List<string>();
            toAppend.Add(Data.currentInstructor.uid + "," + Data.currentInstructor.email + "," + Data.currentInstructor.username + "," + Data.currentInstructor.password + "," + Data.currentInstructor.firstName + "," + Data.currentInstructor.lastName + ",");
            File.AppendAllLines(Application.dataPath + "/Server/Instructors.csv", toAppend);
            SceneManager.LoadScene("LearnersTab");
        }
    }

}
