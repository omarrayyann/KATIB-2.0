using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Linq;

public class AddLearnerScript : MonoBehaviour
{    
    [SerializeField]
    private TMP_Text error = null;


    private void OnEnable()
    {
        // error.gameObject.SetActive(false);
    }

    private bool Validate(TMP_InputField[] fields)
    {
        if (fields[0].text.Length == 0 || fields[1].text.Length == 0 || fields[2].text.Length == 0)
        {
            error.gameObject.SetActive(true);
            error.text = "Make sure required fields aren't empty";
            return false;
        }
        if (!fields[2].text.Contains('@'))
        {
            error.gameObject.SetActive(true);
            error.text = "Make sure the email is in the correct format";
            return false;
        }
        /*if (!Directory.Exists(Application.dataPath + "/Server/Learners.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Learners.csv");
        }*/
        string[] users = Server.ReadAllStringsFrom(SpreadSheets.Learners);
        for (int i = 0; i < users.Length; i++)
        {
            string[] entry = users[i].Split(',');
            if (entry[3].Equals(fields[2].text))
            {
                error.gameObject.SetActive(true);
                error.text = "An account with this email already exists. Try signing in";
                return false;
            }
        }
        return true;
    }

    public void clickedAdd(){
        TMP_InputField[] fields = GetComponentsInChildren<TMP_InputField>();
        if (Validate(fields))
        {
            string firstName = fields[0].text;
            string lastName = fields[1].text;
            string email = fields[2].text;
            string generatedPassword = CreateRandomPassword(8);
            string generatedUsername = email.Split('@')[0];
            string uid = System.Guid.NewGuid().ToString();
            /*if (!Directory.Exists(Application.dataPath + "/Server/Learners.csv"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Server/Learners.csv");
            }*/
            string[] learners = Server.ReadAllStringsFrom(SpreadSheets.Learners);
            foreach (string entry in learners)
            {
                string[] cells = entry.Split(',');
                if (cells[4].Equals(generatedUsername))
                {
                    System.Random rd = new System.Random();
                    generatedUsername += rd.Next(0, 9);
                }
            }
            List<string> learnerList = learners.ToList<string>();
            learnerList.Add(uid + "," + firstName + "," + lastName + "," + email + "," + generatedUsername + "," + generatedPassword + "," + System.DateTime.Now + "," + Data.currentInstructor.uid + ",");
            File.WriteAllLines(Application.dataPath + "/Server/Learners.csv", learnerList);
            Learner newLearner = new Learner(uid, firstName, lastName, email, generatedUsername, generatedPassword, System.DateTime.Now, Data.currentInstructor.uid, new List<string>());
            Data.currentInstructor.learnersUIDs.Add(newLearner.uid);
            /*if (!Directory.Exists(Application.dataPath + "/Server/Instructors.csv"))
            {
                Directory.
            (Application.dataPath + "/Server/Instructors.csv");
            }*/
            string[] instructors = Server.ReadAllStringsFrom(SpreadSheets.Instructors);
            for ( int i = 0; i < instructors.Length; i ++)
            {
                if (instructors[i].Contains(Data.currentInstructor.uid))
                {
                    string[] cells = instructors[i].Split(',');
                    cells[6] += uid + " ";
                    instructors[i] = "";
                    foreach (string cell in cells)
                    {
                        instructors[i] += cell + ",";
                    }
                }
            }
            File.WriteAllLines(Application.dataPath + "/Server/Instructors.csv", instructors);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].text = "";
            }
            gameObject.GetComponentInParent<MenuGroup>().Return();
            GameObject.FindGameObjectWithTag("LearnersView").GetComponent<LearnersView>().loadLearners();
        }
    }



    private static string CreateRandomPassword(int passwordLength)
    {
         string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
         char[] chars = new char[passwordLength];
         System.Random rd = new System.Random();

         for (int i = 0; i < passwordLength; i++)
         {
          chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
         }

        return new string(chars);
    }

}

