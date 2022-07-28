using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class EditClassView : MonoBehaviour
{
    [SerializeField]
    public GameObject learner;

    public List<Learner> learners = new List<Learner>();

    public List<string> newLearnersList = new List<string>();

    public List<GameObject> loadedLearners = new List<GameObject>();

    public Class noEdits = null;

    [SerializeField]
    public TMP_InputField className = null;

    private int numberOfLearners = 0;

    public void loadLearners()
    {
        foreach (string id in Data.currentClass.learnerIDs)
        {
            newLearnersList.Add(id);
        }
        for (int i = 0; i < loadedLearners.Count; i++)
        {
            Destroy(loadedLearners[i]);
        }
        loadedLearners = new List<GameObject>();

        learners = Server.GetLearners(Data.currentInstructor.learnersUIDs);
        int amount = learners.Count;
        numberOfLearners = amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject myNewLearner = Instantiate(learner, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            myNewLearner.transform.SetParent(gameObject.transform);
            myNewLearner.transform.localScale = new Vector3(1, 1, 1);


            Button button = myNewLearner.GetComponentInChildren<Button>();
            Learner learnerNow = learners[i];
            UpdateCard(learnerNow.uid, button.gameObject.transform.parent.gameObject);
            button.onClick.AddListener(() => ClickedCard(learnerNow.uid, button.gameObject.transform.parent.gameObject));
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[0].text = learners[i].getName();
            myNewLearner.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Last active: " + learnerNow.LastActive();
            loadedLearners.Add(myNewLearner);
        }

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, Mathf.Ceil((float)(amount + 1) / 4f) * 212);
    }

    private void UpdateCard(string id, GameObject card)
    {
        Image[] icons = card.GetComponentsInChildren<Image>();
        // Debug.Log("Contains " + id + "? = " + newLearnersList.Contains(id));
        // Debug.Log("Contains " + id + "? = " + Data.currentClass.learnerIDs.Contains(id));
        if (newLearnersList.Contains(id))
        {
            icons[icons.Length - 1].transform.localScale = Vector3.one;
        }
        else
        {
            icons[icons.Length - 1].transform.localScale = Vector3.zero;
        }
    }
    public void ClickedCard(string id, GameObject card)
    {
        Image[] icons = card.GetComponentsInChildren<Image>();
        if (newLearnersList.Contains(id))
        {
            newLearnersList.Remove(id);
            icons[icons.Length - 1].transform.localScale = Vector3.zero;
        }
        else
        {
            newLearnersList.Add(id);
            icons[icons.Length - 1].transform.localScale = Vector3.one;
        }
        Debug.Log(Data.currentClass.learnerIDs.Count);
    }

    private void OnEnable()
    {
        if (Data.currentClass != null)
        {
            newLearnersList = new List<string>();
            noEdits = Data.currentClass;
            loadLearners();
            className.text = Data.currentClass.className;
        }
    }

    private bool Validate(TMP_InputField name)
    {
        if (name.text.Length == 0)
        {
            Debug.LogError("Make sure required fields aren't empty");
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
            if (!entry[0].Equals(Data.currentClass.classID) && entry[3].Equals(Data.currentInstructor.uid) && name.text.Equals(entry[1]))
            {
                Debug.LogError("Class name already in use");
                return false;
            }
        }
        return true;
    }

    public void ConfirmEdits()
    {
        if (Validate(className))
        {
            /*if (!Directory.Exists(Application.dataPath + "/Server/Classes.csv"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Server/Classes.csv");
            }*/
            List<Task> tasks = Server.LoadClassesTasks();
            foreach (Task task in tasks)
            {
                task.DeleteTask();
                task.AddClassTask(Data.currentClass);
            }
            string[] classes = Server.ReadAllStringsFrom(SpreadSheets.Classes);
            for (int i = 0; i < classes.Length; i++)
            {
                if (classes[i].Contains(Data.currentClass.classID))
                {
                    Data.classes.Remove(Data.currentClass);
                    Data.currentClass.className = className.text;
                    Data.currentClass.learnerIDs = newLearnersList;
                    string learners = "";
                    foreach (string ID in Data.currentClass.learnerIDs)
                    {
                        learners += ID + " ";
                    }
                    classes[i] = Data.currentClass.classID + "," + Data.currentClass.className + "," + Data.currentClass.dateCreated + "," + Data.currentClass.instructorID + "," + learners + ",";
                    Data.classes.Add(Data.currentClass);
                }
            }
            File.WriteAllLines(Application.dataPath + "/Server/Classes.csv", classes);
        }
    }
}
