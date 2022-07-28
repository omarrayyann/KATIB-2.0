using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectingTaskAssign : MonoBehaviour
{

    [SerializeField]
    public Toggle noHapticToggle;
    [SerializeField]
    public Toggle partialHapticToggle;
    [SerializeField]
    public Toggle fullHapticToggle;

    [SerializeField]
    public Toggle straightLinesToggle;
    [SerializeField]
    public Toggle parabolasToggle;
    [SerializeField]
    public Toggle randomLinesToggle;


    [SerializeField]
    public Toggle timedToggle;
    [SerializeField]
    public TMP_InputField timeField;
    [SerializeField]
    public TMP_InputField quantityField;

    int hapticLevel = 0;
    int pathsType = 0;
    bool timed = false;
    int time = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (hapticLevel == 0)
        {
            noHapticToggle.isOn = true;
        }
        else if (hapticLevel == 1)
        {
            partialHapticToggle.isOn = true;
        }
        else if (hapticLevel == 2)
        {
            fullHapticToggle.isOn = true;
        }


        if (pathsType == 1)
        {
            straightLinesToggle.isOn = true;
        }
        else if (pathsType == 2)
        {
            parabolasToggle.isOn = true;
        }
        else if (pathsType == 3)
        {
            randomLinesToggle.isOn = true;
        }


        timeField.GetComponent<TMP_InputField>().enabled = false;
        // quantityField.GetComponent<TMP_InputField>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        timed = timedToggle.isOn;

        if (timed)
        {
            timeField.GetComponent<TMP_InputField>().enabled = true;
        }
        else
        {
            timeField.text = "";
            timeField.GetComponent<TMP_InputField>().enabled = false;
        }
    }

    public void toggledPathsType()
    {
        if (straightLinesToggle.isOn == true)
        {
            pathsType = 1;
        }
        else if (parabolasToggle.isOn == true)
        {
            pathsType = 2;
        }
        else if (randomLinesToggle.isOn == true)
        {
            pathsType = 3;
        }
    }

    public void toggledHaptic()
    {
        if (partialHapticToggle.isOn == true)
        {
            hapticLevel = 1;
        }
        else if (noHapticToggle.isOn == true)
        {
            hapticLevel = 0;
        }
        else if (fullHapticToggle.isOn == true)
        {
            hapticLevel = 2;
        }
    }


    public void AssignTask()
    {
        if ((timed && timeField.text == "") || quantityField.text == "")
            Debug.LogError("Missing Info");
        else
        {
            Task task = new Task("Collecting Task");
            int time = timed ? int.Parse(timeField.text) : 0;
            task.collectTask(System.DateTime.Now, timed, time, hapticLevel, int.Parse(quantityField.text), pathsType);
            task.AddTask();
            Server.AddTask(task.taskID);
        }
    }

    public void AssignTaskToClass()
    {
        if ((timed && timeField.text == "") || quantityField.text == "")
            Debug.LogError("Missing Info");
        else
        {
            Task task = new Task("Collecting Task");
            int time = timed ? int.Parse(timeField.text) : 0;
            task.collectTask(System.DateTime.Now, timed, time, hapticLevel, int.Parse(quantityField.text), pathsType);
            task.isClassTask = true;
            task.AddClassTask(Data.currentClass);
        }
    }
}
