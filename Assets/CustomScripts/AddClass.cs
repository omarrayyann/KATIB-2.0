using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class AddClass : MonoBehaviour
{
    [SerializeField]
    private TMP_Text error = null;

    [SerializeField]
    private PickLearners picker = null;

    //[SerializeField]
    //private ClassesView classesView = null;


    private void OnEnable()
    {
        error.gameObject.SetActive(false);
    }

    private bool Validate(TMP_InputField[] fields)
    {
        if (fields[0].text.Length == 0)
        {
            error.gameObject.SetActive(true);
            error.text = "Make sure required fields aren't empty";
            return false;
        }
        /*if (!Directory.Exists(Application.dataPath + "/Server/Classes.csv"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Server/Classes.csv");
        }*/
        string[] classes = Server.ReadAllStringsFrom(SpreadSheets.Classes);
        for (int i = 0; i < classes.Length; i++)
        {
            string[] entry = classes[i].Split(',');
            // Debug.Log(entry.Length);
            if (entry[3].Equals(Data.currentInstructor.uid) && fields[0].text.Equals(entry[1]))
            {
                error.gameObject.SetActive(true);
                error.text = "Class name already in use";
                return false;
            }
        }
        return true;
    }

    public void clickedAdd()
    {
        TMP_InputField[] fields = GetComponentsInChildren<TMP_InputField>();
        Debug.Log(fields.Length);
        if (Validate(fields))
        {
            string className = fields[0].text;
            string classID = System.Guid.NewGuid().ToString();
            string learners = "";
            System.DateTime now = System.DateTime.Now;
            foreach (string id in picker.learnersToAdd)
            {
                learners += id + " ";
            }
            learners += ","; 
            /*if (!Directory.Exists(Application.dataPath + "/Server/Classes.csv"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Server/Classes.csv");
            }*/
            List<string> classes = new List<string>();
            // Debug.Log(learners);
            classes.Add(classID + "," + className + "," + now + "," + Data.currentInstructor.uid + "," + learners);
            File.AppendAllLines(Application.dataPath + "/Server/Classes.csv", classes);
            Class newClass = new Class(classID, className, now, Data.currentInstructor.uid, picker.learnersToAdd);
            Data.classes.Add(newClass);
            picker.ClearSelection();
        }
    }
}

